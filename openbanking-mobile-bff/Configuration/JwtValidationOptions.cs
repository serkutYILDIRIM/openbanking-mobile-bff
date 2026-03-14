namespace openbanking_mobile_bff.Configuration;

public sealed class JwtValidationOptions
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string JwksUri { get; set; } = string.Empty;
    public bool ValidateLifetime { get; set; }
}

