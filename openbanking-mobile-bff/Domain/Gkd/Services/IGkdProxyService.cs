using openbanking_mobile_bff.Domain.Gkd.Models.Requests;
using openbanking_mobile_bff.Domain.Gkd.Models.Responses;

namespace openbanking_mobile_bff.Domain.Gkd.Services;

public interface IGkdProxyService
{
    Task<GkdTokenResponse> GetAuthorizationCodeAsync(string requestId, string aspspCode, string tppCode, string? queryParams = null);
    Task<GkdTokenResponse> CreateAccessTokenAsync(GkdTokenRequest request, string requestId, string aspspCode, string tppCode);
}

