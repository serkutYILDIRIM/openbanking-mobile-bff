using System.Text.Json.Serialization;

namespace openbanking_mobile_bff.Domain.Gkd.Models.Requests;

public sealed class GkdTokenRequest
{
    [JsonPropertyName("bltmKodu")]
    public string? AuthorizationCode { get; set; }
}

