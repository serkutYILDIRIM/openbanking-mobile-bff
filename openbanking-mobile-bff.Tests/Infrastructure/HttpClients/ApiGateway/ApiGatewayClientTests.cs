using System.Net;
using System.Text;
using openbanking_mobile_bff.Common.Exceptions;
using openbanking_mobile_bff.Infrastructure.HttpClients.ApiGateway;
using openbanking_mobile_bff.Infrastructure.HttpClients.ApiGateway.Dtos;

namespace openbanking_mobile_bff.Tests.Infrastructure.HttpClients.ApiGateway;

public sealed class ApiGatewayClientTests
{
    [Fact]
    public async Task GetAuthorizationCodeAsync_WithQueryParams_BuildsGetRequestWithHeadersAndMapsResponse()
    {
        var handler = new CapturingHttpMessageHandler
        {
            Response = CreateJsonResponse("""
                {
                  "erisimBelirteci": "token-1",
                  "belirtecTur": "Bearer",
                  "gecerlilikSuresi": 3600,
                  "yenilemeBelireteci": "refresh-1",
                  "kapsam": "accounts payments"
                }
                """)
        };
        var client = CreateClient(handler);
        var headers = new Dictionary<string, string>
        {
            ["X-Request-ID"] = "req-123",
            ["X-Aspsp-Code"] = "aspsp-001"
        };

        var result = await client.GetAuthorizationCodeAsync(headers, "state=abc&code=xyz");

        Assert.Equal(HttpMethod.Get, handler.CapturedMethod);
        Assert.Equal("/ohvps/gw/s2.0/gkd/yetki-kodu", handler.CapturedRequestUri?.AbsolutePath);
        Assert.Equal("?state=abc&code=xyz", handler.CapturedRequestUri?.Query);
        Assert.Equal("req-123", handler.CapturedHeaders["X-Request-ID"]);
        Assert.Equal("aspsp-001", handler.CapturedHeaders["X-Aspsp-Code"]);
        Assert.Equal("token-1", result.AccessToken);
        Assert.Equal("Bearer", result.TokenType);
        Assert.Equal(3600, result.ExpiresIn);
        Assert.Equal("refresh-1", result.RefreshToken);
        Assert.Equal("accounts payments", result.Scope);
    }

    [Fact]
    public async Task CreateAccessTokenAsync_WithRequest_BuildsPostRequestWithHeadersAndSerializedBody()
    {
        var handler = new CapturingHttpMessageHandler
        {
            Response = CreateJsonResponse("""
                {
                  "erisimBelirteci": "token-9",
                  "belirtecTur": "Bearer"
                }
                """)
        };
        var client = CreateClient(handler);
        var headers = new Dictionary<string, string>
        {
            ["X-Request-ID"] = "req-456",
            ["X-Tpp-Code"] = "tpp-001"
        };
        var request = new GatewayAuthCodeDto
        {
            AuthorizationCode = "auth-code-1",
            ConsentId = "consent-7"
        };

        var result = await client.CreateAccessTokenAsync(request, headers);

        Assert.Equal(HttpMethod.Post, handler.CapturedMethod);
        Assert.Equal("/ohvps/gw/s2.0/gkd/erisim-belirteci", handler.CapturedRequestUri?.AbsolutePath);
        Assert.Equal("req-456", handler.CapturedHeaders["X-Request-ID"]);
        Assert.Equal("tpp-001", handler.CapturedHeaders["X-Tpp-Code"]);
        Assert.Equal("""{"bltmKodu":"auth-code-1","rizaNo":"consent-7"}""", handler.CapturedBody);
        Assert.Equal("application/json; charset=utf-8", handler.CapturedContentType);
        Assert.Equal("token-9", result.AccessToken);
        Assert.Equal("Bearer", result.TokenType);
    }

    [Fact]
    public async Task CreateAccessTokenAsync_WithNonSuccessResponse_ThrowsDownstreamServiceException()
    {
        var handler = new CapturingHttpMessageHandler
        {
            Response = new HttpResponseMessage(HttpStatusCode.BadGateway)
            {
                Content = new StringContent("gateway failed", Encoding.UTF8, "text/plain")
            }
        };
        var client = CreateClient(handler);

        var exception = await Assert.ThrowsAsync<DownstreamServiceException>(() =>
            client.CreateAccessTokenAsync(new GatewayAuthCodeDto(), new Dictionary<string, string>()));

        Assert.Equal("ApiGateway", exception.ServiceName);
        Assert.Equal(HttpStatusCode.BadGateway, exception.StatusCode);
        Assert.Equal("gateway failed", exception.ErrorMessage);
    }

    private static ApiGatewayClient CreateClient(CapturingHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://gateway.local") });

    private static HttpResponseMessage CreateJsonResponse(string json) =>
        new(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

    private sealed class CapturingHttpMessageHandler : HttpMessageHandler
    {
        public HttpResponseMessage Response { get; set; } = new(HttpStatusCode.OK);

        public HttpMethod? CapturedMethod { get; private set; }
        public Uri? CapturedRequestUri { get; private set; }
        public string? CapturedBody { get; private set; }
        public string? CapturedContentType { get; private set; }
        public Dictionary<string, string> CapturedHeaders { get; } = new();

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CapturedMethod = request.Method;
            CapturedRequestUri = request.RequestUri;
            CapturedContentType = request.Content?.Headers.ContentType?.ToString();

            foreach (var header in request.Headers)
                CapturedHeaders[header.Key] = Assert.Single(header.Value);

            if (request.Content is not null)
                CapturedBody = await request.Content.ReadAsStringAsync(cancellationToken);

            return Response;
        }
    }
}
