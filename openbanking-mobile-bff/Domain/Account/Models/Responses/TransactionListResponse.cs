using System.Text.Json.Serialization;

namespace openbanking_mobile_bff.Domain.Account.Models.Responses;

public sealed class TransactionItem
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

public sealed class TransactionListResponse
{
    [JsonPropertyName("islemler")]
    public List<TransactionItem> Transactions { get; set; } = new();

    [JsonPropertyName("toplamKayitSayisi")]
    public int TotalCount { get; set; }

    [JsonPropertyName("sayfaNo")]
    public int PageNumber { get; set; }

    [JsonPropertyName("sayfaBoyutu")]
    public int PageSize { get; set; }
}

