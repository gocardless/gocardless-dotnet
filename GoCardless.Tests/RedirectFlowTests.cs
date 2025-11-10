using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using GoCardless.Resources;
using GoCardless.Services;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GoCardless.Tests
{
    public class RedirectFlowTests
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
        public async Task ShouldBuildValidRequest()
        {
            var responseFixture = "fixtures/client/create_a_redirect_flow_response.json";
            mockHttp.EnqueueResponse(201, responseFixture);
            var prefilledCustomer = new RedirectFlowCreateRequest.RedirectFlowPrefilledCustomer()
            {
                Email = "frank.osborne@acmeplc.com",
                GivenName = "Frank",
                FamilyName = "Osborne",
            };

            var redirectFlowRequest = new RedirectFlowCreateRequest()
            {
                Description = "Wine boxes",
                SessionToken = "SESS_wSs0uGYMISxzqOBq",
                SuccessRedirectUrl = "https://example.com/pay/confirm",
                PrefilledCustomer = prefilledCustomer,
            };

            var redirectFlowResponse = await client.RedirectFlows.CreateAsync(redirectFlowRequest);

            TestHelpers.AssertResponseCanSerializeBackToFixture(
                redirectFlowResponse,
                responseFixture
            );

            var redirectFlow = redirectFlowResponse.RedirectFlow;
            ClassicAssert.AreEqual(
                "http://pay.gocardless.dev/flow/RE123",
                redirectFlow.RedirectUrl
            );
            mockHttp.AssertRequestMade("POST", "/redirect_flows");
        }
    }
}
