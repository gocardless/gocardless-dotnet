using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using GoCardless.Resources;

namespace GoCardless.Internals
{
  public class RequestSigningHelper
  {
    public RequestSigningHelper(
      string privateKeyPem,
      string httpMethod,
      string host,
      string requestPath,
      string keyId,
      string contentDigest = null,
      int? contentLength = null,
      string contentType = "application/json; charset=utf-8",
      string created = null,
      string nonce = null,
      bool _testMode = false
    )
    {
      nonce = _testMode ? "nonce" : nonce ?? Guid.NewGuid().ToString();
      // Notice we expect seconds since epoch (not milliseconds/nanoseconds) to UTC now.
      created = _testMode ? "123" : created ?? DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
      this.SignatureBase = GetSignatureBase(httpMethod, host, requestPath, contentDigest, contentType, contentLength, keyId, created, nonce);
      this.Signature = GetSignature(privateKeyPem, this.SignatureBase);
      this.GcSignature = SignatureHeader(this.Signature);
      this.GcSignatureInput = SignatureInputHeader(keyId, created, nonce, includeContentParams: contentDigest != null);
    }

    public string SignatureBase { get; private set; }
    public string Signature { get; private set; }
    public string GcSignature { get; private set; }
    public string GcSignatureInput { get; private set; }

    public static void SignRequest(HttpRequestMessage request, RequestSigningSettings requestSigningSettings, string content, bool _testMode = false)
    {
      if (requestSigningSettings.Equals(default(RequestSigningSettings)))
      {
        return;
      }

      var privateKeyPem = requestSigningSettings.PrivateKeyPem;
      var keyId = requestSigningSettings.PublicKeyId;


      int? contentLength = null;
      string contentDigest = null;
      if (content != null)
      {
        contentLength = content.Length;
        contentDigest = HashWithSHA256(content);
      }

      var requestUri = request.RequestUri ?? throw new InvalidOperationException("Request URI is null");

      var signatureHelper = new RequestSigningHelper(
        privateKeyPem: privateKeyPem,
        httpMethod: request.Method.ToString(),
        host: requestUri.Host,
        requestPath: requestUri.PathAndQuery,
        keyId: keyId,
        contentDigest: contentDigest,
        contentLength: contentLength,
        _testMode: _testMode
      );

      request.Headers.Add("Gc-Signature", signatureHelper.GcSignature);
      request.Headers.Add("Gc-Signature-Input", signatureHelper.GcSignatureInput);
      if (contentDigest != null)
      {
        request.Headers.Add("Content-Digest", $"sha256=:{contentDigest}:");
      }
    }

    public void VerifySignature(string publicKeyPem, string signature, string signatureBase)
    {
      var signatureBytes = Convert.FromBase64String(signature);

      using (StringReader stringReader = new StringReader(publicKeyPem))
      {
        var publicKey = (AsymmetricKeyParameter)new PemReader(stringReader).ReadObject();
        var verifier = SignerUtilities.GetSigner("SHA-512withECDSA");
        verifier.Init(false, publicKey);

        var messageBytes = GetMessageBytes(signatureBase);
        verifier.BlockUpdate(messageBytes, 0, messageBytes.Length);

        if (!verifier.VerifySignature(signatureBytes))
        {
          throw new Exception("Signature verification failed");
        }
      }
    }

    public static string HashWithSHA256(string value)
    {
      using (SHA256 sha256 = SHA256.Create())
      {
        return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(value)));
      }
    }

    private string SignatureHeader(string signature)
    {
      return $"sig-1=:#{signature}:";
    }

    private string SignatureInputHeader(string keyId, string created, string nonce, bool includeContentParams)
      => $"sig-1={SignatureParams(keyId, created, nonce, includeContentParams)}";

    private string GetSignatureBase(string httpMethod, string host, string requestPath, string contentDigest, string contentType, int? contentLength, string keyId, string created, string nonce)
      => ($"\"@method\": {httpMethod}\n" +
            $"\"@authority\": {host}\n" +
            $"\"@request-target\": {requestPath}\n" +
            (string.IsNullOrWhiteSpace(contentDigest) ? "" :
              $"\"content-digest\": sha256=:{contentDigest}:\n" +
              $"\"content-type\": {contentType}\n" +
              $"\"content-length\": {contentLength}\n") +
            $"\"@signature-params\": {SignatureParams(keyId, created, nonce, includeContentParams: contentDigest != null)}")
            .Trim();

    private string SignatureParams(string keyId, string created, string nonce, bool includeContentParams)
    {
      return @"(""@method"" ""@authority"" ""@request-target""" +
        (includeContentParams ? @" ""content-digest"" ""content-type"" ""content-length""" : "") +
        $@");keyid=""{keyId}"";created={created};nonce=""{nonce}""";
    }

    private string GetSignature(string privateKeyPem, string message)
    {
      using (StringReader stringReader = new StringReader(privateKeyPem))
      {
        var keys = (AsymmetricCipherKeyPair)new PemReader(stringReader).ReadObject();
        var signer = SignerUtilities.GetSigner("SHA-512withECDSA");
        var privateKey = keys.Private;
        signer.Init(true, privateKey);

        var messageBytes = GetMessageBytes(message);
        signer.BlockUpdate(messageBytes, 0, messageBytes.Length);

        var signature = signer.GenerateSignature();

        return Convert.ToBase64String(signature);
      }
    }

    private byte[] GetMessageBytes(string message) => Encoding.UTF8.GetBytes(message);
  }
}
