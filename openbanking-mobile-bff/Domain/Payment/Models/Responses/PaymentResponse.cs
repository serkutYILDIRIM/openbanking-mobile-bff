using System.Text.Json.Serialization;

namespace openbanking_mobile_bff.Domain.Payment.Models.Responses;

public sealed class PaymentResponse
{
    [JsonPropertyName("rzBlg")]
    public PaymentConsentInfo? ConsentInfo { get; set; }

    [JsonPropertyName("katilimciBlg")]
    public PaymentParticipantInfo? ParticipantInfo { get; set; }

    [JsonPropertyName("odmEmr")]
    public PaymentOrderInfo? PaymentOrder { get; set; }
}

public sealed class PaymentConsentInfo
{
    [JsonPropertyName("rizaNo")]
    public string? ConsentId { get; set; }

    [JsonPropertyName("rizaDrm")]
    public string? ConsentStatus { get; set; }
}

public sealed class PaymentParticipantInfo
{
    [JsonPropertyName("hhsKod")]
    public string? HhsCode { get; set; }

    [JsonPropertyName("yosKod")]
    public string? YosCode { get; set; }
}

public sealed class PaymentOrderInfo
{
    [JsonPropertyName("odmEmrNo")]
    public string? PaymentOrderId { get; set; }

    [JsonPropertyName("odmEmrZmn")]
    public DateTime? PaymentOrderTime { get; set; }

    [JsonPropertyName("odmDrm")]
    public string? PaymentStatus { get; set; }

    [JsonPropertyName("islTtr")]
    public PaymentAmountInfo? Amount { get; set; }
}

public sealed class PaymentAmountInfo
{
    [JsonPropertyName("ttr")]
    public string? Amount { get; set; }

    [JsonPropertyName("prBrm")]
    public string? CurrencyCode { get; set; }
}

