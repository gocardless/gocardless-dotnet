using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GoCardless.Services;
using NUnit.Framework;

namespace GoCardless.Tests
{
    public class IdempotencyKeyTests
    {
        private GoCardlessClient client;
        private MockHttp http;

        [SetUp]
        public void SetUp()
        {
            http = new MockHttp();
            client = GoCardlessClient.Create(
                "access-token",
                "https://api.example.com",
                new HttpClient(http)
            );
        }

        [Test]
        public async Task ShouldNotWriteTheGeneratedKeyBackOntoTheRequest()
        {
            // The request object belongs to the caller. Leaving the generated key on it would
            // make a later call that reuses the object send the first call's key, which the
            // API answers with a 409 that this client resolves by returning the first call's
            // resource.
            http.EnqueueResponse(201, "fixtures/client/create_a_mandate_response.json");
            var request = new MandateCreateRequest();

            await client.Mandates.CreateAsync(request);

            Assert.That(request.IdempotencyKey, Is.Null);
        }

        [Test]
        public async Task ShouldSendAFreshKeyWhenARequestObjectIsReused()
        {
            http.EnqueueResponse(201, "fixtures/client/create_a_mandate_response.json");
            http.EnqueueResponse(201, "fixtures/client/create_a_mandate_response.json");
            var request = new MandateCreateRequest();

            await client.Mandates.CreateAsync(request);
            await client.Mandates.CreateAsync(request);

            string first = null;
            string second = null;
            http.AssertRequestMade(
                "POST",
                "/mandates",
                null,
                req => first = req.Item1.Headers.GetValues("Idempotency-Key").Single()
            );
            http.AssertRequestMade(
                "POST",
                "/mandates",
                null,
                req => second = req.Item1.Headers.GetValues("Idempotency-Key").Single()
            );

            Assert.That(first, Is.Not.Null);
            Assert.That(second, Is.Not.Null);
            Assert.That(second, Is.Not.EqualTo(first));
        }

        [Test]
        public async Task ShouldUseAnExplicitKeyAndLeaveItOnTheRequest()
        {
            http.EnqueueResponse(201, "fixtures/client/create_a_mandate_response.json");
            var request = new MandateCreateRequest { IdempotencyKey = "key-set-by-the-caller" };

            await client.Mandates.CreateAsync(request);

            http.AssertRequestMade(
                "POST",
                "/mandates",
                null,
                req =>
                    Assert.That(
                        req.Item1.Headers.GetValues("Idempotency-Key").Single(),
                        Is.EqualTo("key-set-by-the-caller")
                    )
            );
            Assert.That(request.IdempotencyKey, Is.EqualTo("key-set-by-the-caller"));
        }

        [Test]
        public async Task ShouldReuseAnExplicitKeyAcrossCallsWithTheSameRequest()
        {
            // An explicit key is the caller saying these are the same operation, so it is sent
            // as given each time. Only a key this client generated is per-call.
            http.EnqueueResponse(201, "fixtures/client/create_a_mandate_response.json");
            http.EnqueueResponse(201, "fixtures/client/create_a_mandate_response.json");
            var request = new MandateCreateRequest { IdempotencyKey = "key-set-by-the-caller" };

            await client.Mandates.CreateAsync(request);
            await client.Mandates.CreateAsync(request);

            foreach (var unused in new[] { 1, 2 })
            {
                http.AssertRequestMade(
                    "POST",
                    "/mandates",
                    null,
                    req =>
                        Assert.That(
                            req.Item1.Headers.GetValues("Idempotency-Key").Single(),
                            Is.EqualTo("key-set-by-the-caller")
                        )
                );
            }
        }
    }
}
