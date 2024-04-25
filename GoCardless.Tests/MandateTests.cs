using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GoCardless.Resources;
using GoCardless.Services;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using FluentAssertions;

namespace GoCardless.Tests
{
    public class MandateTests
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
        public async Task ShouldListMandatesForACustomer()
        {
            var responseFixture = "fixtures/client/list_mandates_for_a_customer.json";
            mockHttp.EnqueueResponse(200, responseFixture);
            var mandateListRequest = new MandateListRequest()
            {
                Customer = "CU00003068FG73",
                CreatedAt = new MandateListRequest.CreatedAtParam
                {
                    GreaterThan = new DateTimeOffset(2017, 5, 2, 11, 12, 13, TimeSpan.FromHours(-5))
                }
            };
            var listResponse = await client.Mandates.ListAsync(mandateListRequest);
            TestHelpers.AssertResponseCanSerializeBackToFixture(listResponse, responseFixture);
            var mandates = listResponse.Mandates;
            ClassicAssert.AreEqual(2, mandates.Count);
            ClassicAssert.AreEqual("MD00001PEYCSQF", mandates[0].Id);
            ClassicAssert.AreEqual("CR000035EME9H5", mandates[0].Links.Creditor);
            ClassicAssert.AreEqual("MD00001P57AN84", mandates[1].Id);
            ClassicAssert.AreEqual("CR000035EME9H5", mandates[1].Links.Creditor);
            mockHttp.AssertRequestMade("GET",
                "/mandates?created_at%5Bgt%5D=2017-05-02T11%3A12%3A13.0000000-05%3A00&customer=CU00003068FG73");
        }

        [Test]
        public async Task ShouldListMandatesForAUtcDateTimeFilter()
        {
            var responseFixture = "fixtures/client/list_mandates_for_a_customer.json";
            mockHttp.EnqueueResponse(200, responseFixture);
            var mandateListRequest = new MandateListRequest()
            {
                CreatedAt = new MandateListRequest.CreatedAtParam
                {
                    LessThan = DateTime.SpecifyKind(new DateTime(2017, 8, 23, 11, 09, 07), DateTimeKind.Utc)
                }
            };
            var listResponse = await client.Mandates.ListAsync(mandateListRequest);
            mockHttp.AssertRequestMade("GET", "/mandates?created_at%5Blt%5D=2017-08-23T11%3A09%3A07.0000000%2B00%3A00");
        }

        [Test]
        public async Task ShouldListMandatesForADateTimeFilterWithPositiveOffset()
        {
            var responseFixture = "fixtures/client/list_mandates_for_a_customer.json";
            mockHttp.EnqueueResponse(200, responseFixture);
            var mandateListRequest = new MandateListRequest()
            {
                CreatedAt = new MandateListRequest.CreatedAtParam
                {
                    LessThan = new DateTimeOffset(2017, 8, 23, 11, 09, 07, TimeSpan.FromHours(1))
                }
            };
            var listResponse = await client.Mandates.ListAsync(mandateListRequest);
            mockHttp.AssertRequestMade("GET", "/mandates?created_at%5Blt%5D=2017-08-23T11%3A09%3A07.0000000%2B01%3A00");
        }

        [Test]
        public void ShouldListMandatesByCustomerAndStatus()
        {
            var responseFixture = "fixtures/client/list_mandates_by_customer_and_status.json";
            mockHttp.EnqueueResponse(200, responseFixture);
            var mandateListResponse = client.Mandates.ListAsync(new MandateListRequest()
            {
                Customer = "CU00003068FG73",
                Status = new[] {MandateListRequest.MandateStatus.Active, MandateListRequest.MandateStatus.Failed}
            }).Result;
            TestHelpers.AssertResponseCanSerializeBackToFixture(mandateListResponse, responseFixture);

            var mandates =
                mandateListResponse.Mandates;

            ClassicAssert.AreEqual(mandates.Count, 1);
            ClassicAssert.AreEqual(mandates[0].Id, "MD00001PEYCSQF");
            mockHttp.AssertRequestMade("GET", "/mandates?customer=CU00003068FG73&status=active%2Cfailed");
        }

        [Test]
        public void ShouldNotDoubleEncodeUrlParameters()
        {
            var responseFixture = "fixtures/client/list_mandates_by_customer_and_status.json";
            mockHttp.EnqueueResponse(200, responseFixture);
            var mandateListResponse = client.Mandates.ListAsync(new MandateListRequest()
            {
                Customer = "CU00003068FG73",
                Status = new[] {MandateListRequest.MandateStatus.Active, MandateListRequest.MandateStatus.Failed},
                After = "id:MD123"
            }).Result;
            TestHelpers.AssertResponseCanSerializeBackToFixture(mandateListResponse, responseFixture);

            var mandates =
                mandateListResponse.Mandates;

            ClassicAssert.AreEqual(mandates.Count, 1);
            ClassicAssert.AreEqual(mandates[0].Id, "MD00001PEYCSQF");
            mockHttp.AssertRequestMade("GET", "/mandates?after=id%3AMD123&customer=CU00003068FG73&status=active%2Cfailed");
        }


        [Test]
        public void ShouldPageThroughMandates()
        {
            mockHttp.EnqueueResponse(200, "fixtures/client/list_mandates_page_1.json");
            var page1 = client.Mandates.ListAsync(new MandateListRequest() {Limit = 2}).Result;
            page1.Mandates.Should().HaveCount(2);
            page1.Mandates[0].Id.Should().Be("MD00001PEYCSQF");
            page1.Mandates[1].Id.Should().Be("MD00001P57AN84");
            page1.Meta.Cursors.Before.Should().BeNull();
            page1.Meta.Cursors.After.Should().NotBeNullOrEmpty();
            page1.Meta.Limit.Should().Be(2);
            mockHttp.AssertRequestMade("GET", "/mandates?limit=2");
            mockHttp.EnqueueResponse(200, "fixtures/client/list_mandates_page_2.json");
            var page2 =
                client.Mandates.ListAsync(new MandateListRequest {Limit = 2, After = page1.Meta.Cursors.After}).Result;
            page2.Mandates.Should().HaveCount(1);
            page2.Mandates[0].Id.Should().Be("MD00001P1KTRNY");
            page2.Meta.Cursors.Before.Should().NotBe(null);
            page2.Meta.Cursors.After.Should().Be(null);
            page2.Meta.Limit.Should().Be(2);
            mockHttp.AssertRequestMade("GET", "/mandates?after=MD00001P57AN84&limit=2");
        }

        [Test]
        public void ShouldIterateThroughMandates()
        {
            mockHttp.EnqueueResponse(200, "fixtures/client/list_mandates_page_1.json");
            mockHttp.EnqueueResponse(200, "fixtures/client/list_mandates_page_2.json");
            var mandates = client.Mandates.All(new MandateListRequest { Limit = 2 }).ToArray();
            mandates.Count().Should().Be(3);
            ClassicAssert.AreEqual(mandates[0].Id, "MD00001PEYCSQF");
            ClassicAssert.AreEqual(mandates[1].Id, "MD00001P57AN84");
            ClassicAssert.AreEqual(mandates[2].Id, "MD00001P1KTRNY");
            mockHttp.AssertRequestMade("GET", "/mandates?limit=2");
            mockHttp.AssertRequestMade("GET", "/mandates?after=MD00001P57AN84&limit=2");
        }

        [Test]
        public void ShouldIterateThroughMandatesAsync()
        {
            mockHttp.EnqueueResponse(200, "fixtures/client/list_mandates_page_1.json");
            mockHttp.EnqueueResponse(200, "fixtures/client/list_mandates_page_2.json");
            var mandates = client.Mandates.AllAsync(new MandateListRequest { Limit = 2 }).SelectMany(t => t.Result).ToArray();
            mandates.Count().Should().Be(3);
            ClassicAssert.AreEqual(mandates[0].Id, "MD00001PEYCSQF");
            ClassicAssert.AreEqual(mandates[1].Id, "MD00001P57AN84");
            ClassicAssert.AreEqual(mandates[2].Id, "MD00001P1KTRNY");
            mockHttp.AssertRequestMade("GET", "/mandates?limit=2");
            mockHttp.AssertRequestMade("GET", "/mandates?after=MD00001P57AN84&limit=2");
        }

        [Test]
        public async Task ShouldCancelAMandate()
        {
            var responseFixture = "fixtures/client/cancel_a_mandate_response.json";
            mockHttp.EnqueueResponse(200, responseFixture);
            var mandateResponse = await client.Mandates.CancelAsync("MD00001P1KTRNY", new MandateCancelRequest()
            {
                Metadata = new Metadata {{"foo", "bar"}}
            });
            TestHelpers.AssertResponseCanSerializeBackToFixture(mandateResponse, responseFixture);
            var mandate = mandateResponse.Mandate;
            mandate.NextPossibleChargeDate.Should().BeNull();
            mockHttp.AssertRequestMade("POST", "/mandates/MD00001P1KTRNY/actions/cancel",
                "fixtures/client/cancel_a_mandate_request.json");
        }

        [Test]
        public async Task ShouldCreateAMandate()
        {
            var responseFixture = "fixtures/client/create_a_mandate_response.json";
            mockHttp.EnqueueResponse(201, responseFixture,
                resp => resp.Headers.Location = new Uri("/mandates/MD000126", UriKind.Relative));
            MandateResponse mandateResponse = await client.Mandates.CreateAsync(TestHelpers.CreateMandateCreateRequest());

            ClassicAssert.AreEqual(new DateTimeOffset(2017, 06, 19, 17, 01, 06, TimeSpan.FromHours(3)),
                mandateResponse.Mandate.CreatedAt, "DateTimeOffset not correct");

            TestHelpers.AssertResponseCanSerializeBackToFixture(mandateResponse, responseFixture);
            var mandate = mandateResponse.Mandate;
            mockHttp.AssertRequestMade("POST", "/mandates", "fixtures/client/create_a_mandate_request.json");
        }
    }
}
