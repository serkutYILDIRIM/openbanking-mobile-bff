using openbanking_mobile_bff.Domain.Gkd.Models.Requests;
using openbanking_mobile_bff.Domain.Gkd.Services;
using openbanking_mobile_bff.Infrastructure.HttpClients.ApiGateway;
using openbanking_mobile_bff.Infrastructure.HttpClients.ApiGateway.Dtos;

namespace openbanking_mobile_bff.Tests.Domain.Gkd.Services;

public sealed class GkdProxyServiceTests
{
    [Fact]
    public async Task GetAuthorizationCodeAsync_WithAnyInput_ThrowsNotImplementedException()
    {
        var service = new GkdProxyService(new FakeApiGatewayClient());

        await Assert.ThrowsAsync<NotImplementedException>(() =>
            service.GetAuthorizationCodeAsync("req", "aspsp", "tpp", "state=abc"));
    }

    [Fact]
    public async Task CreateAccessTokenAsync_WithAnyInput_ThrowsNotImplementedException()
    {
        var service = new GkdProxyService(new FakeApiGatewayClient());

        await Assert.ThrowsAsync<NotImplementedException>(() =>
            service.CreateAccessTokenAsync(new GkdTokenRequest(), "req", "aspsp", "tpp"));
    }

    private sealed class FakeApiGatewayClient : IApiGatewayClient
    {
        public Task<GatewayAccessTokenDto> GetAuthorizationCodeAsync(Dictionary<string, string> headers, string? queryParams = null) =>
            throw new NotImplementedException();

        public Task<GatewayAccessTokenDto> CreateAccessTokenAsync(GatewayAuthCodeDto request, Dictionary<string, string> headers) =>
            throw new NotImplementedException();
    }
}