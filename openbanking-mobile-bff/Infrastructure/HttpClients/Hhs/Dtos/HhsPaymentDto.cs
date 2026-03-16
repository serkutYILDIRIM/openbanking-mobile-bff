using System.Text.Json.Serialization;

namespace openbanking_mobile_bff.Infrastructure.HttpClients.Hhs.Dtos;

public sealed class HhsPaymentDto
{
    [JsonPropertyName("odmEmriNo")]
    public string? PaymentOrderId { get; set; }

    [JsonPropertyName("odmDrm")]
    public string? PaymentStatus { get; set; }

    [JsonPropertyName("odmTtr")]
    public string? Amount { get; set; }

    [JsonPropertyName("prBrm")]
    public string? CurrencyCode { get; set; }

    [JsonPropertyName("gndAd")]
    public string? SenderName { get; set; }

    [JsonPropertyName("gndHspNo")]
    public string? SenderAccountNumber { get; set; }

    [JsonPropertyName("alcAd")]
    public string? ReceiverName { get; set; }

    [JsonPropertyName("alcHspNo")]
    public string? ReceiverAccountNumber { get; set; }

    [JsonPropertyName("odmAcklm")]
    public string? Description { get; set; }

    [JsonPropertyName("odmZmn")]
    public DateTime? PaymentDateTime { get; set; }
}

