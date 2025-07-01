
namespace GoCardless.Resources
{
  public struct RequestSigningSettings
  {
    public string PublicKeyId { get; set; }
    public string PrivateKeyPem { get; set; }
  }
}