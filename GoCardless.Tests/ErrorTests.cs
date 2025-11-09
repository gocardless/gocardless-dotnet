using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GoCardless.Errors;
using GoCardless.Exceptions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

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
            mockHttp.EnqueueResponse(
                403,
                responseFixture,
                resp => resp.Headers.Location = new Uri("/mandates/MD000126", UriKind.Relative)
            );
            try
            {
                await MakeSomeRequest();
            }
            catch (InsufficientPermissionsException ex)
            {
                TestHelpers.AssertResponseCanSerializeBackToFixture(
                    ex.ApiErrorResponse,
                    responseFixture
                );
                ClassicAssert.AreEqual(ApiErrorType.INSUFFICIENT_PERMISSIONS, ex.Type);
                ClassicAssert.AreEqual("Insufficient permissions", ex.Message);
                ClassicAssert.AreEqual("Insufficient permissions", ex.Errors.Single().Message);
                ClassicAssert.AreEqual("insufficient_permissions", ex.Errors.Single().Reason);
                ClassicAssert.AreEqual(
                    "https://developer.gocardless.com/api-reference#insufficient_permissions",
                    ex.DocumentationUrl
                );
                ClassicAssert.AreEqual("b0e48853-abcd-41fa-9554-5f71820e915d", ex.RequestId);
                ClassicAssert.AreEqual(403, ex.Code);
                return;
            }
            Assert.Fail("Exception was not thrown");
        }

        [Test]
        public async Task ValidationErrors()
        {
            var responseFixture = "fixtures/validation_failed.json";
            mockHttp.EnqueueResponse(
                422,
                responseFixture,
                resp => resp.Headers.Location = new Uri("/mandates/MD000126", UriKind.Relative)
            );
            try
            {
                await MakeSomeRequest();
            }
            catch (ValidationFailedException ex)
            {
                TestHelpers.AssertResponseCanSerializeBackToFixture(
                    ex.ApiErrorResponse,
                    responseFixture
                );
                ClassicAssert.AreEqual(ApiErrorType.VALIDATION_FAILED, ex.Type);
                ClassicAssert.AreEqual("Validation failed", ex.Message);
                ClassicAssert.AreEqual("scheme", ex.Errors[0].Field);
                ClassicAssert.AreEqual(
                    "must be one of bacs, sepa_core, autogiro",
                    ex.Errors[0].Message
                );
                ClassicAssert.AreEqual("/mandates/scheme", ex.Errors[0].RequestPointer);
                ClassicAssert.AreEqual(
                    "https://developer.gocardless.com/api-reference#validation_failed",
                    ex.DocumentationUrl
                );
                ClassicAssert.AreEqual("2f33a336-abcd-4aeb-85c0-101286065dfd", ex.RequestId);
                ClassicAssert.AreEqual(422, ex.Code);
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
                ClassicAssert.AreEqual(500, ex.Code);
                ClassicAssert.AreEqual(ApiErrorType.GOCARDLESS, ex.Type);
                ClassicAssert.AreEqual(
                    "Something went wrong with this request. Please check the ResponseMessage property.",
                    ex.Message
                );
                ClassicAssert.AreEqual(
                    "500 Internal Server Error",
                    ex.ResponseMessage.Content.ReadAsStringAsync().Result
                );
                return;
            }
            Assert.Fail("Exception was not thrown");
        }

        [Test]
        public async Task HtmlErrorPage()
        {
            var responseFixture = "fixtures/error_page.html";
            mockHttp.EnqueueResponse(502, responseFixture);
            try
            {
                await MakeSomeRequest();
            }
            catch (ApiException ex)
            {
                ClassicAssert.AreEqual(502, ex.Code);
                ClassicAssert.AreEqual(ApiErrorType.GOCARDLESS, ex.Type);
                ClassicAssert.AreEqual(
                    "Something went wrong with this request. Please check the ResponseMessage property.",
                    ex.Message
                );
                ClassicAssert.AreEqual(
                    "<!DOCTYPE html>\n<html>\n  <head>\n    <title>This is an HTML error page, like one returned by Cloudflare</title>\n  </head>\n</html>\n",
                    ex.ResponseMessage.Content.ReadAsStringAsync().Result.Replace("\r\n", "\n")
                );
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
            var mandate = await this.client.Mandates.CreateAsync(
                TestHelpers.CreateMandateCreateRequest()
            );
            //Then the mandate should be successfully retrieved.
            ClassicAssert.AreEqual("BA000123", mandate.Mandate.Links.CustomerBankAccount);
        }

        [Test]
        public async Task WhenAuthenticationError_ShouldShowSpecificError()
        {
            //Authentication Errors should throw a specific more contextual type rather than generic invalid_api_usage

            //Arrange
            var responseFixture = "fixtures/authentication_failure.json";
            mockHttp.EnqueueResponse(
                401,
                responseFixture,
                resp => resp.Headers.Location = new Uri("/mandates/MD000126", UriKind.Relative)
            );

            //Act

            try
            {
                await MakeSomeRequest();
            }
            catch (AuthenticationFailedException ex)
            {
                ClassicAssert.AreEqual(401, ex.Code);
                ClassicAssert.AreEqual(ApiErrorType.AUTHENTICATION_FAILED, ex.Type);
                ClassicAssert.AreEqual("Authentication Failed", ex.Message);
                ClassicAssert.AreEqual("Authentication Failed", ex.Errors.Single().Message);
                ClassicAssert.AreEqual("authentication_failed", ex.Errors.Single().Reason);
                TestHelpers.AssertResponseCanSerializeBackToFixture(
                    ex.ApiErrorResponse,
                    responseFixture
                );
                ClassicAssert.AreEqual(
                    "https://developer.gocardless.com/api-reference/#api-usage-errors",
                    ex.DocumentationUrl
                );
                return;
            }

            //Assert
            Assert.Fail("Exception was not thrown");
        }

        [Test]
        public async Task WhenRateLimitReachedException_ShouldShowSpecificError()
        {
            //Rate Limit being reached errors should throw a specific more contextual type rather than generic
            //Arrange
            var responseFixture = "fixtures/rate_limit_reached.json";
            mockHttp.EnqueueResponse(
                429,
                responseFixture,
                resp => resp.Headers.Location = new Uri("/mandates/MD000126", UriKind.Relative)
            );

            //Act
            try
            {
                await MakeSomeRequest();
            }
            catch (RateLimitReachedException ex)
            {
                ClassicAssert.AreEqual(429, ex.Code);
                ClassicAssert.AreEqual(ApiErrorType.RATE_LIMIT_REACHED, ex.Type);
                ClassicAssert.AreEqual(
                    "Rate Limit Reached you have made too many requests",
                    ex.Message
                );
                ClassicAssert.AreEqual(
                    "Rate Limit Reached you have made too many requests",
                    ex.Errors.Single().Message
                );
                ClassicAssert.AreEqual("rate_limit_reached", ex.Errors.Single().Reason);
                TestHelpers.AssertResponseCanSerializeBackToFixture(
                    ex.ApiErrorResponse,
                    responseFixture
                );
                ClassicAssert.AreEqual(
                    "https://developer.gocardless.com/api-reference/#making-requests-rate-limiting",
                    ex.DocumentationUrl
                );
                return;
            }

            //Assert
            Assert.Fail("Exception was not thrown");
        }
    }
}
