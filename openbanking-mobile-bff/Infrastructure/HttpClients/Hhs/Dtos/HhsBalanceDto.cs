using System.Text.Json.Serialization;

namespace openbanking_mobile_bff.Infrastructure.HttpClients.Hhs.Dtos;

public sealed class HhsBalanceDto
{
    [JsonPropertyName("hspRef")]
    public string? AccountRef { get; set; }

    [JsonPropertyName("bkye")]
    public string? Amount { get; set; }

    [JsonPropertyName("prBrm")]
    public string? CurrencyCode { get; set; }

    [JsonPropertyName("bkyeTrh")]
    public DateTime? BalanceDateTime { get; set; }

    [JsonPropertyName("bkyeTur")]
    public string? BalanceType { get; set; }
}

