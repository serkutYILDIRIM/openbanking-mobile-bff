using openbanking_mobile_bff.Infrastructure.HttpClients.ApiGateway.Dtos;

namespace openbanking_mobile_bff.Infrastructure.HttpClients.ApiGateway;

public interface IApiGatewayClient
{
    Task<GatewayAccessTokenDto> GetAuthorizationCodeAsync(Dictionary<string, string> headers, string? queryParams = null);
    Task<GatewayAccessTokenDto> CreateAccessTokenAsync(GatewayAuthCodeDto request, Dictionary<string, string> headers);
}

