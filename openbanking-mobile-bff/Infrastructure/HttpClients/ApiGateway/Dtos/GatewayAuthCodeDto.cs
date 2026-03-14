using System.Text.Json.Serialization;

namespace openbanking_mobile_bff.Infrastructure.HttpClients.ApiGateway.Dtos;

public sealed class GatewayAuthCodeDto
{
    [JsonPropertyName("bltmKodu")]
    public string? AuthorizationCode { get; set; }

    [JsonPropertyName("rizaNo")]
    public string? ConsentId { get; set; }
}

