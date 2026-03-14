using System.Text.Json.Serialization;

namespace openbanking_mobile_bff.Infrastructure.HttpClients.Yos.Dtos;

public sealed class YosConsentDto
{
    [JsonPropertyName("rzBlg")]
    public RizaBilgiDto? ConsentInfo { get; set; }

    [JsonPropertyName("katilimciBlg")]
    public KatilimciBlgDto? ParticipantInfo { get; set; }

    [JsonPropertyName("gkd")]
    public GkdDto? Gkd { get; set; }

    [JsonPropertyName("kmlk")]
    public KimlikDto? Identity { get; set; }

    [JsonPropertyName("hspBlg")]
    public HesapBilgiDto? AccountInfo { get; set; }

    [JsonPropertyName("odmBlg")]
    public OdemeBlgDto? PaymentInfo { get; set; }
}

public sealed class RizaBilgiDto
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
    public IptalBilgiDto? CancelDetail { get; set; }
}

public sealed class IptalBilgiDto
{
    [JsonPropertyName("iptalKod")]
    public string? CancelCode { get; set; }

    [JsonPropertyName("iptalAciklama")]
    public string? CancelDescription { get; set; }
}

public sealed class KatilimciBlgDto
{
    [JsonPropertyName("hhsKod")]
    public string? HhsCode { get; set; }

    [JsonPropertyName("yosKod")]
    public string? YosCode { get; set; }
}

public sealed class GkdDto
{
    [JsonPropertyName("yetYntm")]
    public string? AuthMethod { get; set; }

    [JsonPropertyName("yonAdr")]
    public string? RedirectUri { get; set; }

    [JsonPropertyName("hhsYonAdr")]
    public string? HhsRedirectUri { get; set; }

    [JsonPropertyName("bltmKodu")]
    public string? AuthCode { get; set; }
}

public sealed class KimlikDto
{
    [JsonPropertyName("kmlkTur")]
    public string? IdentityType { get; set; }

    [JsonPropertyName("kmlkVrs")]
    public string? IdentityValue { get; set; }

    [JsonPropertyName("krmKmlkTur")]
    public string? CorporateIdentityType { get; set; }

    [JsonPropertyName("krmKmlkVrs")]
    public string? CorporateIdentityValue { get; set; }

    [JsonPropertyName("ohkTur")]
    public string? CustomerType { get; set; }
}

public sealed class HesapBilgiDto
{
    [JsonPropertyName("iznTur")]
    public List<string>? PermissionTypes { get; set; }

    [JsonPropertyName("erisimIzniSonTrh")]
    public DateTime? AccessValidUntil { get; set; }

    [JsonPropertyName("hesapNo")]
    public string? AccountNumber { get; set; }
}

public sealed class OdemeBlgDto
{
    [JsonPropertyName("odmBsltm")]
    public OdemeBaslatmaDto? PaymentInitiation { get; set; }

    [JsonPropertyName("islTtr")]
    public ParaDto? Amount { get; set; }
}

public sealed class OdemeBaslatmaDto
{
    [JsonPropertyName("kmlk")]
    public KimlikDto? Identity { get; set; }

    [JsonPropertyName("gon")]
    public TarafDto? Sender { get; set; }

    [JsonPropertyName("alc")]
    public TarafDto? Receiver { get; set; }
}

public sealed class TarafDto
{
    [JsonPropertyName("unv")]
    public string? Title { get; set; }

    [JsonPropertyName("hspNo")]
    public string? AccountNumber { get; set; }
}

public sealed class ParaDto
{
    [JsonPropertyName("ttr")]
    public string? Amount { get; set; }

    [JsonPropertyName("prBrm")]
    public string? CurrencyCode { get; set; }
}

