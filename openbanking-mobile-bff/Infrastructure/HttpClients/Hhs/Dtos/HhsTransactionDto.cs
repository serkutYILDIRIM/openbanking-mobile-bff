using System.Text.Json.Serialization;

namespace openbanking_mobile_bff.Infrastructure.HttpClients.Hhs.Dtos;

public sealed class HhsTransactionDto
{
    [JsonPropertyName("islmNo")]
    public string? TransactionId { get; set; }

    [JsonPropertyName("islmTtr")]
    public string? Amount { get; set; }

    [JsonPropertyName("prBrm")]
    public string? CurrencyCode { get; set; }

    [JsonPropertyName("islmTrh")]
    public DateTime? TransactionDate { get; set; }

    [JsonPropertyName("acklm")]
    public string? Description { get; set; }

    [JsonPropertyName("brcAlc")]
    public string? CreditorDebitIndicator { get; set; }
}

