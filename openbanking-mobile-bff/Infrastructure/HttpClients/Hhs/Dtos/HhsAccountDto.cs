using System.Text.Json.Serialization;

namespace openbanking_mobile_bff.Infrastructure.HttpClients.Hhs.Dtos;

public sealed class HhsAccountDto
{
    [JsonPropertyName("hspRef")]
    public string? AccountRef { get; set; }

    [JsonPropertyName("hspNo")]
    public string? AccountNumber { get; set; }

    [JsonPropertyName("hspSahibi")]
    public string? AccountOwner { get; set; }

    [JsonPropertyName("hspTur")]
    public string? AccountType { get; set; }

    [JsonPropertyName("prBrm")]
    public string? CurrencyCode { get; set; }

    [JsonPropertyName("subeKod")]
    public string? BranchCode { get; set; }

    [JsonPropertyName("hspDurum")]
    public string? AccountStatus { get; set; }
}

