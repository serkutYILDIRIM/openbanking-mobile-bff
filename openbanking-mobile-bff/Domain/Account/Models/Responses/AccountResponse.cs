using System.Text.Json.Serialization;

namespace openbanking_mobile_bff.Domain.Account.Models.Responses;

public sealed class AccountResponse
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

public sealed class AccountListResponse
{
    [JsonPropertyName("hesaplar")]
    public List<AccountResponse> Accounts { get; set; } = new();

    [JsonPropertyName("toplamKayitSayisi")]
    public int TotalCount { get; set; }

    [JsonPropertyName("sayfaNo")]
    public int PageNumber { get; set; }

    [JsonPropertyName("sayfaBoyutu")]
    public int PageSize { get; set; }
}

