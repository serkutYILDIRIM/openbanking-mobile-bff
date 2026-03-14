using System.Text.Json.Serialization;

namespace openbanking_mobile_bff.Infrastructure.HttpClients.Yos.Dtos;

public sealed class YosGkdDto
{
    [JsonPropertyName("bltmKodu")]
    public string? AuthorizationCode { get; set; }

    [JsonPropertyName("erisimBelirteci")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("belirtecTur")]
    public string? TokenType { get; set; }

    [JsonPropertyName("gecerlilikSuresi")]
    public int? ExpiresIn { get; set; }

    [JsonPropertyName("yenilemeBelireteci")]
    public string? RefreshToken { get; set; }

    [JsonPropertyName("kapsam")]
    public string? Scope { get; set; }
}

