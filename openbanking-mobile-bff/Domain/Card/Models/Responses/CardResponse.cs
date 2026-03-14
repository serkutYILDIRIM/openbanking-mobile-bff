using System.Text.Json.Serialization;

namespace openbanking_mobile_bff.Domain.Card.Models.Responses;

public sealed class CardResponse
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

public sealed class CardListResponse
{
    [JsonPropertyName("kartlar")]
    public List<CardResponse> Cards { get; set; } = new();

    [JsonPropertyName("toplamKayitSayisi")]
    public int TotalCount { get; set; }
}

public sealed class CardTransactionResponse
{
    [JsonPropertyName("krtRef")]
    public string? CardRef { get; set; }

    [JsonPropertyName("islmTrh")]
    public DateTime? TransactionDate { get; set; }

    [JsonPropertyName("islmTtr")]
    public string? Amount { get; set; }

    [JsonPropertyName("isyeriAdi")]
    public string? MerchantName { get; set; }

    [JsonPropertyName("acklm")]
    public string? Description { get; set; }
}

