using System.Text.Json.Serialization;

namespace openbanking_mobile_bff.Domain.Payment.Models.Requests;

public sealed class PaymentRequest
{
    [JsonPropertyName("katilimciBlg")]
    public PaymentParticipantInfo? ParticipantInfo { get; set; }

    [JsonPropertyName("gkd")]
    public PaymentGkd? Gkd { get; set; }

    [JsonPropertyName("odmBlg")]
    public PaymentInfo? PaymentInfo { get; set; }
}

public sealed class PaymentParticipantInfo
{
    [JsonPropertyName("hhsKod")]
    public string? HhsCode { get; set; }

    [JsonPropertyName("yosKod")]
    public string? YosCode { get; set; }
}

public sealed class PaymentGkd
{
    [JsonPropertyName("yetYntm")]
    public string? AuthMethod { get; set; }

    [JsonPropertyName("yonAdr")]
    public string? RedirectUri { get; set; }
}

public sealed class PaymentInfo
{
    [JsonPropertyName("odmBsltm")]
    public PaymentInitiation? PaymentInitiation { get; set; }

    [JsonPropertyName("islTtr")]
    public PaymentAmount? Amount { get; set; }
}

public sealed class PaymentInitiation
{
    [JsonPropertyName("kmlk")]
    public PaymentIdentity? Identity { get; set; }

    [JsonPropertyName("gon")]
    public PaymentParty? Sender { get; set; }

    [JsonPropertyName("alc")]
    public PaymentParty? Receiver { get; set; }
}

public sealed class PaymentIdentity
{
    [JsonPropertyName("kmlkTur")]
    public string? IdentityType { get; set; }

    [JsonPropertyName("kmlkVrs")]
    public string? IdentityValue { get; set; }

    [JsonPropertyName("ohkTur")]
    public string? CustomerType { get; set; }
}

public sealed class PaymentParty
{
    [JsonPropertyName("unv")]
    public string? Title { get; set; }

    [JsonPropertyName("hspNo")]
    public string? AccountNumber { get; set; }
}

public sealed class PaymentAmount
{
    [JsonPropertyName("ttr")]
    public string? Amount { get; set; }

    [JsonPropertyName("prBrm")]
    public string? CurrencyCode { get; set; }
}

