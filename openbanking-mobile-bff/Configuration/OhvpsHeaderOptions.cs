namespace openbanking_mobile_bff.Configuration;

public sealed class OhvpsHeaderOptions
{
    public string RequestIdHeader { get; set; } = "X-Request-ID";
    public string AspspCodeHeader { get; set; } = "X-ASPSP-Code";
    public string TppCodeHeader { get; set; } = "X-TPP-Code";
    public string JwsSignatureHeader { get; set; } = "X-JWS-Signature";
}

