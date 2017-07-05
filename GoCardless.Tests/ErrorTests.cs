using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using GoCardless.Errors;
using GoCardless.Exceptions;

namespace GoCardless.Tests
{
    public class ErrorTests
    {
        private GoCardlessClient client;
        public MockHttp mockHttp;

        [SetUp]
        public void SetUp()
        {
            mockHttp = new MockHttp();
            var httpClient = new HttpClient(mockHttp);
            client = GoCardlessClient.Create("access-token", "https://api.example.com", httpClient);
        }

        [Test]
        public async Task InsufficentPermissions()
        {
            var responseFixture = "fixtures/insufficient_permissions.json";
            mockHttp.EnqueueResponse(403, responseFixture, resp => resp.Headers.Location = new Uri("/mandates/MD000126", UriKind.Relative));
            try
            {
                await MakeSomeRequest();
            }
            catch (InvalidApiUsageException ex)
            {
                TestHelpers.AssertResponseCanSerializeBackToFixture(ex.ApiErrorResponse, responseFixture);
                Assert.AreEqual(ApiErrorType.INVALID_API_USAGE, ex.Type);
                Assert.AreEqual("Insufficient permissions", ex.Message);
                Assert.AreEqual("Insufficient permissions", ex.Errors.Single().Message);
                Assert.AreEqual("insufficient_permissions", ex.Errors.Single().Reason);
                Assert.AreEqual("insufficient_permissions", ex.Errors.Single().Reason);
                Assert.AreEqual("https://developer.gocardless.com/api-reference#insufficient_permissions",
                    ex.DocumentationUrl);
                Assert.AreEqual("b0e48853-abcd-41fa-9554-5f71820e915d", ex.RequestId);
                Assert.AreEqual(403, ex.Code);
                return;
            }
            Assert.Fail("Exception was not thrown");
        }

        [Test]
        public async Task ValidationErrors()
        {
            var responseFixture = "fixtures/validation_failed.json";
            mockHttp.EnqueueResponse(422, responseFixture,
                resp => resp.Headers.Location = new Uri("/mandates/MD000126", UriKind.Relative));
            try
            {
                await MakeSomeRequest();
            }
            catch (ValidationFailedException ex)
            {
                TestHelpers.AssertResponseCanSerializeBackToFixture(ex.ApiErrorResponse, responseFixture);
                Assert.AreEqual(ApiErrorType.VALIDATION_FAILED, ex.Type);
                Assert.AreEqual("Validation failed", ex.Message);
                Assert.AreEqual("scheme", ex.Errors[0].Field);
                Assert.AreEqual("must be one of bacs, sepa_core, autogiro", ex.Errors[0].Message);
                Assert.AreEqual("/mandates/scheme", ex.Errors[0].RequestPointer);
                Assert.AreEqual("https://developer.gocardless.com/api-reference#validation_failed",
                    ex.DocumentationUrl);
                Assert.AreEqual("2f33a336-abcd-4aeb-85c0-101286065dfd", ex.RequestId);
                Assert.AreEqual(422, ex.Code);
                return;
            }
            Assert.Fail("Exception was not thrown");
        }

        private async Task MakeSomeRequest()
        {
            await client.Mandates.CreateAsync(TestHelpers.CreateMandateCreateRequest());
        }


        [Test]
        public async Task TrueServerError()
        {
            var responseFixture = "fixtures/server_error.json";
            mockHttp.EnqueueResponse(500, responseFixture);
            try
            {
                await MakeSomeRequest();
            }
            catch (ApiException ex)
            {
                Assert.AreEqual(500, ex.Code);
                Assert.AreEqual(ApiErrorType.GOCARDLESS, ex.Type);
                Assert.AreEqual("Something went wrong with this request. Please check the ResponseMessage property.", ex.Message);
                Assert.AreEqual("500 Internal Server Error", ex.ResponseMessage.Content.ReadAsStringAsync().Result);
                return;
            }
            Assert.Fail("Exception was not thrown");
        }

        [Test]
        public async Task IdempotencyConflictsAreHandledAutomatically()
        {
            //Given a mandate has already been created using the idempotency key
            mockHttp.EnqueueResponse(409, "fixtures/idempotent_creation_conflict.json");
            mockHttp.EnqueueResponse(200, "fixtures/client/create_a_mandate_response.json");
            //When an attempt to created the mandate is made
            var mandate = await this.client.Mandates.CreateAsync(TestHelpers.CreateMandateCreateRequest());
            //Then the mandate should be successfully retrieved.
            Assert.AreEqual("BA000123", mandate.Mandate.Links.CustomerBankAccount);
        }

    }
}
