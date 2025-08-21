using System;
using System.Collections.Generic;
using System.IO;
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
    public class OutboundPaymentServiceTests
    {
        private GoCardlessClient client;
        public MockHttp mockHttp;

        [SetUp]
        public void SetUp()
        {
            mockHttp = new MockHttp();
            var httpClient = new HttpClient(mockHttp);
            var requestSigningSettings = new RequestSigningSettings
            {
                PrivateKeyPem = File.ReadAllText("fixtures/client/request_signing/private_key.pem"),
                PublicKeyId = "PublicKeyId",
            };
            client = GoCardlessClient.Create("access-token", requestSigningSettings, baseUrl: "https://api.example.com", client: httpClient);
            client._testMode = true; // Enable test mode for deterministic signatures
        }

        [Test]
        public async Task ShouldSignOutboundPaymentsGet()
        {
            var responseFixture = "fixtures/client/outbound_payment_service/get_response.json";

            mockHttp.EnqueueResponse(200, responseFixture);
            var resp = await client.OutboundPayments.GetAsync("OUT123");

            mockHttp.AssertRequestMade("GET", "/outbound_payments/OUT123", null, r =>
            {
                var request = r.Item1;
                ClassicAssert.AreEqual(@"sig-1=(""@method"" ""@authority"" ""@request-target"");keyid=""PublicKeyId"";created=123;nonce=""nonce""", request.Headers.GetValues("Gc-Signature-Input").FirstOrDefault());
                ClassicAssert.True(request.Headers.Contains("Gc-Signature"));
                ClassicAssert.True(request.Headers.GetValues("Gc-Signature").First().StartsWith("sig-1=:"));
                ClassicAssert.True(request.Headers.GetValues("Gc-Signature").First().EndsWith(":"));
                ClassicAssert.False(request.Headers.Contains("Content-Digest"));
            });
            TestHelpers.AssertResponseCanSerializeBackToFixture(resp, responseFixture);
        }

        [Test]
        public async Task ShouldSignOutboundPaymentsList()
        {
            var responseFixture = "fixtures/client/outbound_payment_service/index_response.json";
            mockHttp.EnqueueResponse(200, responseFixture);

            var resp = await client.OutboundPayments.ListAsync(new OutboundPaymentListRequest
            {
                CreatedFrom = "2025-06-27",
                CreatedTo = "2025-06-27",
            });

            mockHttp.AssertRequestMade("GET", "/outbound_payments?created_from=2025-06-27&created_to=2025-06-27", null, r =>
            {
                var request = r.Item1;
                ClassicAssert.AreEqual(@"sig-1=(""@method"" ""@authority"" ""@request-target"");keyid=""PublicKeyId"";created=123;nonce=""nonce""", request.Headers.GetValues("Gc-Signature-Input").FirstOrDefault());
                ClassicAssert.True(request.Headers.Contains("Gc-Signature"));
                ClassicAssert.True(request.Headers.GetValues("Gc-Signature").First().StartsWith("sig-1=:"));
                ClassicAssert.True(request.Headers.GetValues("Gc-Signature").First().EndsWith(":"));
                ClassicAssert.False(request.Headers.Contains("Content-Digest"));
            });
            TestHelpers.AssertResponseCanSerializeBackToFixture(resp, responseFixture);
        }

        [Test]
        public void ShouldSignOutboundPaymentsAll()
        {
            var responseFixture = "fixtures/client/outbound_payment_service/index_response.json";
            mockHttp.EnqueueResponse(200, responseFixture);

            var resp = client.OutboundPayments.All(new OutboundPaymentListRequest
            {
                CreatedFrom = "2025-06-27",
                CreatedTo = "2025-06-27",
            }).ToList();

            mockHttp.AssertRequestMade("GET", "/outbound_payments?created_from=2025-06-27&created_to=2025-06-27", null, r =>
            {
                var request = r.Item1;
                ClassicAssert.AreEqual(@"sig-1=(""@method"" ""@authority"" ""@request-target"");keyid=""PublicKeyId"";created=123;nonce=""nonce""", request.Headers.GetValues("Gc-Signature-Input").FirstOrDefault());
                ClassicAssert.True(request.Headers.Contains("Gc-Signature"));
                ClassicAssert.True(request.Headers.GetValues("Gc-Signature").First().StartsWith("sig-1=:"));
                ClassicAssert.True(request.Headers.GetValues("Gc-Signature").First().EndsWith(":"));
                ClassicAssert.False(request.Headers.Contains("Content-Digest"));
            });
        }

        [Test]
        public async Task ShouldSignOutboundPaymentsCreate()
        {
            var responseFixture = "fixtures/client/outbound_payment_service/create_response.json";
            mockHttp.EnqueueResponse(200, responseFixture);

            var resp = await client.OutboundPayments.CreateAsync(new OutboundPaymentCreateRequest
            {
                Amount = 100,
                Scheme = OutboundPaymentCreateRequest.OutboundPaymentScheme.FasterPayments,
                Links = new OutboundPaymentCreateRequest.OutboundPaymentLinks
                {
                    RecipientBankAccount = "BA0000XZDKC8VY",
                }
            });

            mockHttp.AssertRequestMade("POST", "/outbound_payments", null, r =>
            {
                var request = r.Item1;
                ClassicAssert.AreEqual(@"sig-1=(""@method"" ""@authority"" ""@request-target"" ""content-digest"" ""content-type"" ""content-length"");keyid=""PublicKeyId"";created=123;nonce=""nonce""", request.Headers.GetValues("Gc-Signature-Input").FirstOrDefault());
                ClassicAssert.True(request.Headers.Contains("Gc-Signature"));
                ClassicAssert.True(request.Headers.GetValues("Gc-Signature").First().StartsWith("sig-1=:"));
                ClassicAssert.True(request.Headers.GetValues("Gc-Signature").First().EndsWith(":"));
                ClassicAssert.True(request.Headers.Contains("Content-Digest"));
                ClassicAssert.True(request.Headers.GetValues("Content-Digest").First().StartsWith("sha256=:"));
                ClassicAssert.True(request.Headers.GetValues("Content-Digest").First().EndsWith(":"));
            });
            TestHelpers.AssertResponseCanSerializeBackToFixture(resp, responseFixture);
        }

        [Test]
        public async Task ShouldSignOutboundPaymentsApprove()
        {
            var responseFixture = "fixtures/client/outbound_payment_service/approve_response.json";
            mockHttp.EnqueueResponse(200, responseFixture);

            var resp = await client.OutboundPayments.ApproveAsync("OUT123");

            mockHttp.AssertRequestMade("POST", "/outbound_payments/OUT123/actions/approve", null, r =>
            {
                var request = r.Item1;
                ClassicAssert.AreEqual(@"sig-1=(""@method"" ""@authority"" ""@request-target"" ""content-digest"" ""content-type"" ""content-length"");keyid=""PublicKeyId"";created=123;nonce=""nonce""", request.Headers.GetValues("Gc-Signature-Input").FirstOrDefault());
                ClassicAssert.True(request.Headers.Contains("Gc-Signature"));
                ClassicAssert.True(request.Headers.GetValues("Gc-Signature").First().StartsWith("sig-1=:"));
                ClassicAssert.True(request.Headers.GetValues("Gc-Signature").First().EndsWith(":"));
                ClassicAssert.True(request.Headers.Contains("Content-Digest"));
                ClassicAssert.True(request.Headers.GetValues("Content-Digest").First().StartsWith("sha256=:"));
                ClassicAssert.True(request.Headers.GetValues("Content-Digest").First().EndsWith(":"));
            });
            TestHelpers.AssertResponseCanSerializeBackToFixture(resp, responseFixture);
        }

        [Test]
        public async Task ShouldSignOutboundPaymentsCancel()
        {
            var responseFixture = "fixtures/client/outbound_payment_service/cancel_response.json";
            mockHttp.EnqueueResponse(200, responseFixture);

            var resp = await client.OutboundPayments.CancelAsync("OUT123");

            mockHttp.AssertRequestMade("POST", "/outbound_payments/OUT123/actions/cancel", null, r =>
            {
                var request = r.Item1;
                ClassicAssert.AreEqual(@"sig-1=(""@method"" ""@authority"" ""@request-target"" ""content-digest"" ""content-type"" ""content-length"");keyid=""PublicKeyId"";created=123;nonce=""nonce""", request.Headers.GetValues("Gc-Signature-Input").FirstOrDefault());
                ClassicAssert.True(request.Headers.Contains("Gc-Signature"));
                ClassicAssert.True(request.Headers.GetValues("Gc-Signature").First().StartsWith("sig-1=:"));
                ClassicAssert.True(request.Headers.GetValues("Gc-Signature").First().EndsWith(":"));
                ClassicAssert.True(request.Headers.Contains("Content-Digest"));
                ClassicAssert.True(request.Headers.GetValues("Content-Digest").First().StartsWith("sha256=:"));
                ClassicAssert.True(request.Headers.GetValues("Content-Digest").First().EndsWith(":"));
            });
            TestHelpers.AssertResponseCanSerializeBackToFixture(resp, responseFixture);
        }
    }
}
