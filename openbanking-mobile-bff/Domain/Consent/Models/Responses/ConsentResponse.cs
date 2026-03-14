using System.Text.Json.Serialization;

namespace openbanking_mobile_bff.Domain.Consent.Models.Responses;

public sealed class ConsentResponse
{
    [JsonPropertyName("rzBlg")]
    public ConsentInfo? ConsentInfo { get; set; }

    [JsonPropertyName("katilimciBlg")]
    public ConsentResponseParticipant? ParticipantInfo { get; set; }

    [JsonPropertyName("gkd")]
    public ConsentResponseGkd? Gkd { get; set; }

    [JsonPropertyName("kmlk")]
    public ConsentResponseIdentity? Identity { get; set; }
}

public sealed class ConsentInfo
{
    [JsonPropertyName("rizaNo")]
    public string? ConsentId { get; set; }

    [JsonPropertyName("olusturmaZamani")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("guncellemeZamani")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("rizaDrm")]
    public string? ConsentStatus { get; set; }

    [JsonPropertyName("iptalBilgi")]
    public ConsentCancelDetail? CancelDetail { get; set; }
}

public sealed class ConsentCancelDetail
{
    [JsonPropertyName("iptalKod")]
    public string? CancelDetailCode { get; set; }

    [JsonPropertyName("iptalAciklama")]
    public string? CancelDescription { get; set; }
}

public sealed class ConsentResponseParticipant
{
    [JsonPropertyName("hhsKod")]
    public string? HhsCode { get; set; }

    [JsonPropertyName("yosKod")]
    public string? YosCode { get; set; }
}

public sealed class ConsentResponseGkd
{
    [JsonPropertyName("yetYntm")]
    public string? AuthMethod { get; set; }

    [JsonPropertyName("yonAdr")]
    public string? RedirectUri { get; set; }

    [JsonPropertyName("hhsYonAdr")]
    public string? HhsRedirectUri { get; set; }
}

public sealed class ConsentResponseIdentity
{
    [JsonPropertyName("kmlkTur")]
    public string? IdentityType { get; set; }

    [JsonPropertyName("kmlkVrs")]
    public string? IdentityValue { get; set; }

    [JsonPropertyName("ohkTur")]
    public string? CustomerType { get; set; }
}

