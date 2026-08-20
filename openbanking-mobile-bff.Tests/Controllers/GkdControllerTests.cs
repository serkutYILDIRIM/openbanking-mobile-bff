using Microsoft.AspNetCore.Mvc;
using openbanking_mobile_bff.Controllers;
using openbanking_mobile_bff.Domain.Gkd.Models.Requests;
using openbanking_mobile_bff.Domain.Gkd.Models.Responses;
using openbanking_mobile_bff.Domain.Gkd.Services;

namespace openbanking_mobile_bff.Tests.Controllers;

public sealed class GkdControllerTests
{
    [Fact]
    public async Task GetAuthorizationCode_ReturnsOkWithResponseAndPassesArgumentsToService()
    {
        var expected = new GkdTokenResponse
        {
            AccessToken = "token-1",
            TokenType = "Bearer"
        };

        var service = new FakeGkdProxyService { GetAuthorizationCodeResult = expected };
        var controller = new GkdController(service);
        var actionResult = await controller.GetAuthorizationCode("req-123", "aspsp-001", "tpp-001", "state=abc");

        var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
        var value = Assert.IsType<GkdTokenResponse>(ok.Value);

        Assert.Same(expected, value);
        Assert.Equal(("req-123", "aspsp-001", "tpp-001", "state=abc"), service.GetAuthorizationCodeArgs);
    }

    [Fact]
    public async Task CreateAccessToken_ReturnsOkWithResponseAndPassesArgumentsToService()
    {
        var request = new GkdTokenRequest { AuthorizationCode = "auth-1" };
        var expected = new GkdTokenResponse
        {
            AccessToken = "token-2",
            RefreshToken = "refresh-1"
        };

        var service = new FakeGkdProxyService { CreateAccessTokenResult = expected };
        var controller = new GkdController(service);
        var actionResult = await controller.CreateAccessToken(request, "req-123", "aspsp-001", "tpp-001");

        var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
        var value = Assert.IsType<GkdTokenResponse>(ok.Value);

        Assert.Same(expected, value);
        Assert.Equal((request, "req-123", "aspsp-001", "tpp-001"), service.CreateAccessTokenArgs);
    }

    private sealed class FakeGkdProxyService : IGkdProxyService
    {
        public GkdTokenResponse GetAuthorizationCodeResult { get; set; } = new();
        public GkdTokenResponse CreateAccessTokenResult { get; set; } = new();

        public (string RequestId, string AspspCode, string TppCode, string? QueryParams)? GetAuthorizationCodeArgs { get; private set; }
        public (GkdTokenRequest Request, string RequestId, string AspspCode, string TppCode)? CreateAccessTokenArgs { get; private set; }

        public Task<GkdTokenResponse> GetAuthorizationCodeAsync(string requestId, string aspspCode, string tppCode, string? queryParams = null)
        {
            GetAuthorizationCodeArgs = (requestId, aspspCode, tppCode, queryParams);
            return Task.FromResult(GetAuthorizationCodeResult);
        }

        public Task<GkdTokenResponse> CreateAccessTokenAsync(GkdTokenRequest request, string requestId, string aspspCode, string tppCode)
        {
            CreateAccessTokenArgs = (request, requestId, aspspCode, tppCode);
            return Task.FromResult(CreateAccessTokenResult);
        }
    }
}

