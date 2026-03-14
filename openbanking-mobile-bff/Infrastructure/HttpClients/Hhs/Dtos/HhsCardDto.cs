using System.Text.Json.Serialization;

namespace openbanking_mobile_bff.Infrastructure.HttpClients.Hhs.Dtos;

public sealed class HhsCardDto
{
    [JsonPropertyName("krtRef")]
    public string? CardRef { get; set; }

    [JsonPropertyName("krtNo")]
    public string? CardNumber { get; set; }

    [JsonPropertyName("krtSahibi")]
    public string? CardHolder { get; set; }

    [JsonPropertyName("krtTur")]
    public string? CardType { get; set; }

    [JsonPropertyName("krtDurum")]
    public string? CardStatus { get; set; }
}

