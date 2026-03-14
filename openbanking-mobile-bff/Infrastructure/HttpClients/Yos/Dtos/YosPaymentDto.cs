using System.Text.Json.Serialization;

namespace openbanking_mobile_bff.Infrastructure.HttpClients.Yos.Dtos;

public sealed class YosPaymentDto
{
    [JsonPropertyName("rzBlg")]
    public RizaBilgiDto? ConsentInfo { get; set; }

    [JsonPropertyName("katilimciBlg")]
    public KatilimciBlgDto? ParticipantInfo { get; set; }

    [JsonPropertyName("gkd")]
    public GkdDto? Gkd { get; set; }

    [JsonPropertyName("odmEmr")]
    public OdemeEmriDto? PaymentOrder { get; set; }
}

public sealed class OdemeEmriDto
{
    [JsonPropertyName("odmEmrNo")]
    public string? PaymentOrderId { get; set; }

    [JsonPropertyName("odmEmrZmn")]
    public DateTime? PaymentOrderTime { get; set; }

    [JsonPropertyName("odmDrm")]
    public string? PaymentStatus { get; set; }

    [JsonPropertyName("islTtr")]
    public ParaDto? Amount { get; set; }

    [JsonPropertyName("gon")]
    public TarafDto? Sender { get; set; }

    [JsonPropertyName("alc")]
    public TarafDto? Receiver { get; set; }

    [JsonPropertyName("odmAyr")]
    public OdemeAyrDto? PaymentDetail { get; set; }
}

public sealed class OdemeAyrDto
{
    [JsonPropertyName("odmKynk")]
    public string? PaymentSource { get; set; }

    [JsonPropertyName("odmAmc")]
    public string? PaymentPurpose { get; set; }

    [JsonPropertyName("refBlg")]
    public string? ReferenceInfo { get; set; }

    [JsonPropertyName("acklm")]
    public string? Description { get; set; }
}

