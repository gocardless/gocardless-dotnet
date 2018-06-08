using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using FluentAssertions;
using GoCardless.Internals;
using GoCardless.Services;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;

namespace GoCardless.Tests
{
    public class FunctionalityTests
    {
        private GoCardlessClient client;
        public MockHttp http ;

        [SetUp]
        public void SetUp()
        {
            http = new MockHttp();
            var httpClient = new HttpClient(http);
            client = GoCardlessClient.Create("access-token", "https://api.example.com", httpClient);
        }

        [Test]
        public async Task CanAccessResponseMessageContentAsync()
        {
            //Given a successful request has been made
            http.EnqueueResponse(200, "fixtures/client/list_mandates_for_a_customer.json");
            var mandateListRequest = new MandateListRequest() { Customer = "CU00003068FG73" };
            var listResponse = client.Mandates.ListAsync(mandateListRequest).Result;
            //When the responseMessage attached to the response is inspected
            //Then the responseMessage content can be read
            listResponse.ResponseMessage.Should().NotBeNull();
            var content = await listResponse.ResponseMessage.Content.ReadAsStringAsync();
            Assert.AreEqual(File.ReadAllText("fixtures/client/list_mandates_for_a_customer.json"), content);
        }

        [Test]
        public async Task Headers()
        {
            http.EnqueueResponse(201, "fixtures/client/create_a_mandate_response.json", resp => resp.Headers.Location = new Uri("/mandates/MD000126", UriKind.Relative));
            var mandateResponse = await client.Mandates.CreateAsync(new MandateCreateRequest());

            var mandate = mandateResponse.Mandate;
            http.AssertRequestMade("POST", "/mandates", null, req =>
            {
                Assert.AreEqual("Bearer access-token", req.Item1.Headers.GetValues("Authorization").Single());
                Assert.AreEqual("2015-07-06", req.Item1.Headers.GetValues("GoCardless-Version").Single());
                Assert.AreEqual("gocardless-dotnet", req.Item1.Headers.GetValues("GoCardless-Client-Library").Single());
            });
        }

        [Test]
        public async Task IdempotencyKeyIsGeneratedWhenNoneIsSet()
        {
            //When a request has been made to an endpoint requiring an idemopotency key without one being set
            http.EnqueueResponse(201, "fixtures/client/create_a_mandate_response.json", resp => resp.Headers.Location = new Uri("/mandates/MD000126", UriKind.Relative));
            await client.Mandates.CreateAsync(new MandateCreateRequest());

            Guid? idempotencyKey = null;
            http.AssertRequestMade("POST", "/mandates", null, req =>
            {
                idempotencyKey = Guid.Parse(req.Item1.Headers.GetValues("Idempotency-Key").Single());
            });

            //Then an idempotency key should have been set automatically by the client library
            Assert.NotNull(idempotencyKey);

        }


        [Test]
        public async Task ClientCanModifyRequestBeforeSending()
        {
            //Given a client wants to modify the raw request before it is sent
            string someValue = Guid.NewGuid().ToString();
            var requestSettings = new RequestSettings
            {
                CustomiseRequestMessage = msg => msg.Headers.Add("SomeModifiedHeader", someValue)
            };

            //When the request is made using the customisation
            http.EnqueueResponse(201, "fixtures/client/create_a_mandate_response.json", resp => resp.Headers.Location = new Uri("/mandates/MD000126", UriKind.Relative));

            await client.Mandates.CreateAsync(new MandateCreateRequest(), requestSettings);

            Guid? customHeader = null;
            http.AssertRequestMade("POST", "/mandates", null, req =>
            {
                customHeader = Guid.Parse(req.Item1.Headers.GetValues("SomeModifiedHeader").Single());
            });

            //Then the modification should have been applied to the request
            Assert.NotNull(customHeader);
        }

        [Test]
        public async Task ClientCanSetCustomHeaders()
        {
            var requestSettings = new RequestSettings
            {
                Headers = new Dictionary<string, string>()
                {
                    {"Accept-Language", "fr"},
                    {"User-Agent", "Skynet"}
                }
            };

            //When the request is made using the customisation
            http.EnqueueResponse(201, "fixtures/client/create_a_mandate_response.json", resp => resp.Headers.Location = new Uri("/mandates/MD000126", UriKind.Relative));

            await client.Mandates.CreateAsync(new MandateCreateRequest(), requestSettings);

            http.AssertRequestMade("POST", "/mandates", null, req =>
            {
                //The brand new header we've set should be there
                Assert.AreEqual(req.Item1.Headers.GetValues("Accept-Language").Single(), "fr");
                //We should still get the default headers set by the client
                Assert.NotNull(req.Item1.Headers.GetValues("Authorization").Single());
                //Headers we set should override the client's default headers
                Assert.AreEqual(req.Item1.Headers.GetValues("User-Agent").Single(), "Skynet");
            });
        }

        [Test]
        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, false)]
        public async Task RequestRetriesUpToThreeTimesOnTimeout(int numberOfFailures, bool requestShouldSucceed)
        {
            //Given a request is going to time out {numberOfFailures} times before succeeding
            for (var i = 0; i < numberOfFailures; i++)
            {
                http.EnqueueResponse(201, "fixtures/client/create_a_mandate_response.json",
                    resp => throw new TaskCanceledException());
            }
            http.EnqueueResponse(201, "fixtures/client/create_a_mandate_response.json");
            string firstIdempotencyKey = null;
            try
            {
                //When the service method is called
                var response = await client.Mandates.CreateAsync(new MandateCreateRequest(), new RequestSettings()
                {
                    CustomiseRequestMessage = req =>
                    {
                        //Then the idempotency keys should stay the same on successive retries
                        var newIdempotencyKey = req.Headers.GetValues("Idempotency-Key").Single();
                        firstIdempotencyKey = firstIdempotencyKey ?? newIdempotencyKey;
                        Assert.NotNull(newIdempotencyKey);
                        Assert.AreEqual(firstIdempotencyKey, newIdempotencyKey, "Idempotency keys must match on retried requests");
                    }
                });
                //And if there were enough retries to handle the failures the call should succeed
                Assert.True(response.ResponseMessage.IsSuccessStatusCode);
            }
            catch (TaskCanceledException)
            {
                //And if not the call should have timed out
                Assert.False(requestShouldSucceed);
            }

        }

        [Test]
        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, false)]
        public async Task RequestRetriesUpToThreeTimesOnConnectionFailure(int numberOfFailures, bool requestShouldSucceed)
        {
        //Given a request is going to time out {numberOfFailures} times before succeeding
            for (var i = 0; i < numberOfFailures; i++)
            {
                http.EnqueueResponse(201, "fixtures/client/create_a_mandate_response.json",
                    resp => throw new HttpRequestException());
            }
            http.EnqueueResponse(201, "fixtures/client/create_a_mandate_response.json");
            string firstIdempotencyKey = null;
            try
            {
                //When the service method is called
                var response = await client.Mandates.CreateAsync(new MandateCreateRequest(), new RequestSettings()
                {
                    CustomiseRequestMessage = req =>
                    {
                        // Then the idempotency keys should stay the same on successive retries
                        var newIdempotencyKey = req.Headers.GetValues("Idempotency-Key").Single();
                        firstIdempotencyKey = firstIdempotencyKey ?? newIdempotencyKey;
                        Assert.NotNull(newIdempotencyKey);
                        Assert.AreEqual(firstIdempotencyKey, newIdempotencyKey, "Idempotency keys must match on retried requests");
                    }
                });
                //And if there were enough retries to handle the failures the call should succeed
                Assert.True(response.ResponseMessage.IsSuccessStatusCode);
            }
            catch (HttpRequestException)
            {
                //And if not the call should have timed out
                Assert.False(requestShouldSucceed);
            }

        }

        [Test]
        [TestCase(0, true)]
        [TestCase(1, true)]
        [TestCase(2, false)]
        public async Task RequestRetriesWithCustomValues(int numberOfFailures, bool shouldBeSuccessful)
        {
            //When a request is going to time out {numberOfFailures} times before succeeding
            for (var i = 0; i < numberOfFailures; i++)
            {
                http.EnqueueResponse(201, "fixtures/client/create_a_mandate_response.json",
                    resp => throw new TaskCanceledException());
            }
            http.EnqueueResponse(201, "fixtures/client/create_a_mandate_response.json");
            bool wasSuccessful = false;
            try
            {
                var response = await client.Mandates.CreateAsync(new MandateCreateRequest(), new RequestSettings()
                {
                    WaitBetweenRetries = TimeSpan.FromSeconds(0.25),
                    NumberOfRetriesOnTimeout = 1
                });
                wasSuccessful = response.ResponseMessage.IsSuccessStatusCode;
            }
            catch (Exception)
            {

            }

            Assert.AreEqual(shouldBeSuccessful, wasSuccessful);

        }

        [Test]
        public async Task BooleansAreDowncasedInQueryString()
        {
            var responseFixture = "fixtures/mock_response.json";
            http.EnqueueResponse(200, responseFixture);
            var listRequest = new CustomerBankAccountListRequest(){ Enabled = true };
            var listResponse = await client.CustomerBankAccounts.ListAsync(listRequest);
            http.AssertRequestMade("GET", "/customer_bank_accounts?enabled=true");
        }
    }
}
