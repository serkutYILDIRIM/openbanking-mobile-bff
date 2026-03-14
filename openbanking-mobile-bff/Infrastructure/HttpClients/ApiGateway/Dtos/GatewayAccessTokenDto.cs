using System.Text.Json.Serialization;

namespace openbanking_mobile_bff.Infrastructure.HttpClients.ApiGateway.Dtos;

public sealed class GatewayAccessTokenDto
{
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

