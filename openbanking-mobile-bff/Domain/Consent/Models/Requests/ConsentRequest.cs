using System.Text.Json.Serialization;

namespace openbanking_mobile_bff.Domain.Consent.Models.Requests;

public sealed class ConsentRequest
{
    [JsonPropertyName("katilimciBlg")]
    public ConsentParticipantInfo? ParticipantInfo { get; set; }

    [JsonPropertyName("gkd")]
    public ConsentGkd? Gkd { get; set; }

    [JsonPropertyName("kmlk")]
    public ConsentIdentity? Identity { get; set; }

    [JsonPropertyName("hspBlg")]
    public ConsentAccountInfo? AccountInfo { get; set; }

    [JsonPropertyName("odmBlg")]
    public ConsentPaymentInfo? PaymentInfo { get; set; }
}

public sealed class ConsentParticipantInfo
{
    [JsonPropertyName("hhsKod")]
    public string? HhsCode { get; set; }

    [JsonPropertyName("yosKod")]
    public string? YosCode { get; set; }
}

public sealed class ConsentGkd
{
    [JsonPropertyName("yetYntm")]
    public string? AuthMethod { get; set; }

    [JsonPropertyName("yonAdr")]
    public string? RedirectUri { get; set; }
}

public sealed class ConsentIdentity
{
    [JsonPropertyName("kmlkTur")]
    public string? IdentityType { get; set; }

    [JsonPropertyName("kmlkVrs")]
    public string? IdentityValue { get; set; }

    [JsonPropertyName("ohkTur")]
    public string? CustomerType { get; set; }
}

public sealed class ConsentAccountInfo
{
    [JsonPropertyName("iznTur")]
    public List<string>? PermissionTypes { get; set; }

    [JsonPropertyName("erisimIzniSonTrh")]
    public DateTime? AccessValidUntil { get; set; }
}

public sealed class ConsentPaymentInfo
{
    [JsonPropertyName("odmBsltm")]
    public ConsentPaymentInitiation? PaymentInitiation { get; set; }

    [JsonPropertyName("islTtr")]
    public ConsentAmount? Amount { get; set; }
}

public sealed class ConsentPaymentInitiation
{
    [JsonPropertyName("kmlk")]
    public ConsentIdentity? Identity { get; set; }

    [JsonPropertyName("gon")]
    public ConsentParty? Sender { get; set; }

    [JsonPropertyName("alc")]
    public ConsentParty? Receiver { get; set; }
}

public sealed class ConsentParty
{
    [JsonPropertyName("unv")]
    public string? Title { get; set; }

    [JsonPropertyName("hspNo")]
    public string? AccountNumber { get; set; }
}

public sealed class ConsentAmount
{
    [JsonPropertyName("ttr")]
    public string? Amount { get; set; }

    [JsonPropertyName("prBrm")]
    public string? CurrencyCode { get; set; }
}

