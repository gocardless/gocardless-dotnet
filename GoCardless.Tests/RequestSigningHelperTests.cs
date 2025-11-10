using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using GoCardless.Internals;
using GoCardless.Resources;
using GoCardless.Resources;
using GoCardless.Services;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GoCardless.Tests
{
    public class RequestSigningHelperTests
    {
        private string privateKeyPem;
        private string publicKeyPem;

        [SetUp]
        public void SetUp()
        {
            privateKeyPem = File.ReadAllText("fixtures/client/request_signing/private_key.pem");
            publicKeyPem = File.ReadAllText("fixtures/client/request_signing/public_key.pem");
        }

        const string SIG_INPUT_NO_CONTENT =
            @"sig-1=(""@method"" ""@authority"" ""@request-target"");keyid=""PublicKeyId"";created=123;nonce=""nonce""";
        const string SIG_INPUT_WITH_CONTENT =
            @"sig-1=(""@method"" ""@authority"" ""@request-target"" ""content-digest"" ""content-type"" ""content-length"");keyid=""PublicKeyId"";created=123;nonce=""nonce""";

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public async Task ShouldGenerateSignature(bool withContent)
        {
            var signer = new RequestSigningHelper(
                privateKeyPem: privateKeyPem,
                httpMethod: "GET",
                host: "api.example.com",
                requestPath: "/test",
                keyId: "PublicKeyId",
                contentType: withContent ? "application/json" : null,
                contentDigest: withContent ? "digest" : null,
                contentLength: withContent ? 321 : null,
                nonce: "nonce",
                created: "123"
            );

            signer.VerifySignature(publicKeyPem, signer.Signature, signer.SignatureBase);
            ClassicAssert.False(string.IsNullOrWhiteSpace(signer.GcSignature));
            ClassicAssert.True(signer.GcSignature.StartsWith("sig-1=:"));
            ClassicAssert.True(signer.GcSignature.EndsWith(":"));
        }

        [Test]
        [TestCase(false, SIG_INPUT_NO_CONTENT)]
        [TestCase(true, SIG_INPUT_WITH_CONTENT)]
        public async Task ShouldGenerateSignatureInput(bool withContent, string expectedInputHeader)
        {
            var signer = new RequestSigningHelper(
                privateKeyPem: privateKeyPem,
                httpMethod: "GET",
                host: "api.example.com",
                requestPath: "/test",
                keyId: "PublicKeyId",
                contentDigest: withContent ? "digest" : null,
                contentLength: withContent ? 321 : null,
                nonce: "nonce",
                created: "123"
            );

            ClassicAssert.AreEqual(expectedInputHeader, signer.GcSignatureInput);
        }

        [Test]
        public async Task ShouldGenerateSha()
        {
            const string content = "test content";

            var digest = RequestSigningHelper.HashWithSHA256(content);

            ClassicAssert.AreEqual("auinVVUgn9bEQVfArtgBbnY/9DWhnPGG92hjFAFD/3I=", digest);
        }

        [Test]
        [TestCase(null, SIG_INPUT_NO_CONTENT, null)]
        [TestCase(
            "test content",
            SIG_INPUT_WITH_CONTENT,
            "sha256=:auinVVUgn9bEQVfArtgBbnY/9DWhnPGG92hjFAFD/3I=:"
        )]
        public async Task ShouldSignRequest(
            string content,
            string expectedSigInput,
            string expectedContentDigestHeader
        )
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.example.com/test");
            var requestSigningSettings = new RequestSigningSettings
            {
                PrivateKeyPem = privateKeyPem,
                PublicKeyId = "PublicKeyId",
            };

            RequestSigningHelper.SignRequest(
                request,
                requestSigningSettings,
                content,
                _testMode: true
            );

            ClassicAssert.True(request.Headers.Contains("Gc-Signature"));
            var gcSig = request.Headers.GetValues("Gc-Signature").FirstOrDefault();
            ClassicAssert.True(gcSig.StartsWith("sig-1=:"));
            ClassicAssert.True(gcSig.EndsWith(":"));

            ClassicAssert.True(request.Headers.Contains("Gc-Signature-Input"));
            var sigInput = request.Headers.GetValues("Gc-Signature-Input").FirstOrDefault();
            ClassicAssert.AreEqual(expectedSigInput, sigInput);

            var haveDigest = request.Headers.Contains("Content-Digest");
            var contentDigest = haveDigest
                ? request.Headers.GetValues("Content-Digest").FirstOrDefault()
                : null;
            ClassicAssert.AreEqual(expectedContentDigestHeader, contentDigest);
        }
    }
}
