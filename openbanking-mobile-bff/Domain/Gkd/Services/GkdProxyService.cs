using openbanking_mobile_bff.Domain.Gkd.Models.Requests;
using openbanking_mobile_bff.Domain.Gkd.Models.Responses;
using openbanking_mobile_bff.Infrastructure.HttpClients.ApiGateway;

namespace openbanking_mobile_bff.Domain.Gkd.Services;

public sealed class GkdProxyService : IGkdProxyService
{
    private readonly IApiGatewayClient _apiGatewayClient;

    public GkdProxyService(IApiGatewayClient apiGatewayClient)
    {
        _apiGatewayClient = apiGatewayClient;
    }

    public Task<GkdTokenResponse> GetAuthorizationCodeAsync(string requestId, string aspspCode, string tppCode, string? queryParams = null)
    {
        throw new NotImplementedException();
    }

    public Task<GkdTokenResponse> CreateAccessTokenAsync(GkdTokenRequest request, string requestId, string aspspCode, string tppCode)
    {
        throw new NotImplementedException();
    }
}

