using openbanking_mobile_bff.Common.Constants;

namespace openbanking_mobile_bff.Common.Utilities;

public static class HttpHeaderUtil
{
    public static Dictionary<string, string> BuildOhvpsHeaders(
        string requestId,
        string aspspCode,
        string tppCode,
        string? jwsSignature)
    {
        var headers = new Dictionary<string, string>
        {
            [OhvpsConstants.RequestIdHeader] = requestId,
            [OhvpsConstants.AspspCodeHeader] = aspspCode,
            [OhvpsConstants.TppCodeHeader] = tppCode
        };

        if (!string.IsNullOrEmpty(jwsSignature))
        {
            headers[OhvpsConstants.JwsSignatureHeader] = jwsSignature;
        }

        return headers;
    }

    public static void PropagateHeaders(HttpRequestMessage target, HttpContext source)
    {
        var headerNames = new[]
        {
            OhvpsConstants.RequestIdHeader,
            OhvpsConstants.AspspCodeHeader,
            OhvpsConstants.TppCodeHeader,
            OhvpsConstants.JwsSignatureHeader,
            OhvpsConstants.IdempotencyKeyHeader
        };

        foreach (var name in headerNames)
        {
            if (source.Request.Headers.TryGetValue(name, out var value))
            {
                target.Headers.TryAddWithoutValidation(name, (string?)value);
            }
        }
    }
}

