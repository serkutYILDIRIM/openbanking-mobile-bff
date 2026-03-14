using System.Text.Json.Serialization;

namespace openbanking_mobile_bff.Domain.Payment.Models.Responses;

public sealed class PaymentStatusResponse
{
    [JsonPropertyName("odmEmrNo")]
    public string? PaymentOrderId { get; set; }

    [JsonPropertyName("odmDrm")]
    public string? PaymentStatus { get; set; }

    [JsonPropertyName("odmEmrZmn")]
    public DateTime? PaymentOrderTime { get; set; }

    [JsonPropertyName("islTtr")]
    public PaymentAmountInfo? Amount { get; set; }
}

