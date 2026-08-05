using System.Net;
using System.Text;
using openbanking_mobile_bff.Common.Exceptions;
using openbanking_mobile_bff.Infrastructure.HttpClients.Yos;
using openbanking_mobile_bff.Infrastructure.HttpClients.Yos.Dtos;

namespace openbanking_mobile_bff.Tests.Infrastructure.HttpClients.Yos;

public sealed class YosMicroserviceClientTests
{
    [Fact]
    public async Task CreateAccountConsentAsync_WithRequest_BuildsPostRequestWithHeadersAndMapsResponse()
    {
        var handler = new CapturingHttpMessageHandler
        {
            Response = CreateJsonResponse("""
                {
                  "rzBlg": { "rizaNo": "riza-1", "rizaDrm": "B" },
                  "katilimciBlg": { "hhsKod": "hhs-1", "yosKod": "yos-1" }
                }
                """)
        };
        var client = CreateClient(handler);
        var headers = new Dictionary<string, string>
        {
            ["X-Request-ID"] = "req-001",
            ["X-Aspsp-Code"] = "aspsp-001"
        };
        var request = new YosConsentDto
        {
            ConsentInfo = new RizaBilgiDto { ConsentId = "riza-1" }
        };

        var result = await client.CreateAccountConsentAsync(request, headers);

        Assert.Equal(HttpMethod.Post, handler.CapturedMethod);
        Assert.Equal("/ohvps/obh/s2.0/hesap-bilgisi-rizasi", handler.CapturedRequestUri?.AbsolutePath);
        Assert.Equal("req-001", handler.CapturedHeaders["X-Request-ID"]);
        Assert.Equal("aspsp-001", handler.CapturedHeaders["X-Aspsp-Code"]);
        Assert.Equal("application/json; charset=utf-8", handler.CapturedContentType);
        Assert.Equal("riza-1", result.ConsentInfo?.ConsentId);
        Assert.Equal("B", result.ConsentInfo?.ConsentStatus);
        Assert.Equal("hhs-1", result.ParticipantInfo?.HhsCode);
        Assert.Equal("yos-1", result.ParticipantInfo?.YosCode);
    }

    [Fact]
    public async Task GetAccountConsentAsync_WithConsentId_BuildsGetRequestWithConsentIdInPath()
    {
        var handler = new CapturingHttpMessageHandler
        {
            Response = CreateJsonResponse("""
                {
                  "rzBlg": { "rizaNo": "riza-42", "rizaDrm": "Y" }
                }
                """)
        };
        var client = CreateClient(handler);
        var headers = new Dictionary<string, string>
        {
            ["X-Request-ID"] = "req-002"
        };

        var result = await client.GetAccountConsentAsync("riza-42", headers);

        Assert.Equal(HttpMethod.Get, handler.CapturedMethod);
        Assert.Equal("/ohvps/obh/s2.0/hesap-bilgisi-rizasi/riza-42", handler.CapturedRequestUri?.AbsolutePath);
        Assert.Equal("req-002", handler.CapturedHeaders["X-Request-ID"]);
        Assert.Equal("riza-42", result.ConsentInfo?.ConsentId);
        Assert.Equal("Y", result.ConsentInfo?.ConsentStatus);
    }

    [Fact]
    public async Task DeleteAccountConsentAsync_WithConsentId_BuildsDeleteRequest()
    {
        var handler = new CapturingHttpMessageHandler
        {
            Response = new HttpResponseMessage(HttpStatusCode.NoContent)
        };
        var client = CreateClient(handler);
        var headers = new Dictionary<string, string>
        {
            ["X-Request-ID"] = "req-003"
        };

        await client.DeleteAccountConsentAsync("riza-5", headers);

        Assert.Equal(HttpMethod.Delete, handler.CapturedMethod);
        Assert.Equal("/ohvps/obh/s2.0/hesap-bilgisi-rizasi/riza-5", handler.CapturedRequestUri?.AbsolutePath);
        Assert.Equal("req-003", handler.CapturedHeaders["X-Request-ID"]);
    }

    [Fact]
    public async Task DeleteAccountConsentAsync_WithNonSuccessResponse_ThrowsDownstreamServiceException()
    {
        var handler = new CapturingHttpMessageHandler
        {
            Response = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("consent not found", Encoding.UTF8, "text/plain")
            }
        };
        var client = CreateClient(handler);

        var exception = await Assert.ThrowsAsync<DownstreamServiceException>(() =>
            client.DeleteAccountConsentAsync("riza-99", new Dictionary<string, string>()));

        Assert.Equal("YosMicroservice", exception.ServiceName);
        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        Assert.Equal("consent not found", exception.ErrorMessage);
    }

    [Fact]
    public async Task CreatePaymentConsentAsync_WithRequest_BuildsPostRequestToCorrectPath()
    {
        var handler = new CapturingHttpMessageHandler
        {
            Response = CreateJsonResponse("""
                {
                  "rzBlg": { "rizaNo": "odeme-riza-1", "rizaDrm": "B" }
                }
                """)
        };
        var client = CreateClient(handler);
        var headers = new Dictionary<string, string>
        {
            ["X-Request-ID"] = "req-004",
            ["X-Tpp-Code"] = "tpp-001"
        };
        var request = new YosConsentDto
        {
            ConsentInfo = new RizaBilgiDto { ConsentId = "odeme-riza-1" }
        };

        var result = await client.CreatePaymentConsentAsync(request, headers);

        Assert.Equal(HttpMethod.Post, handler.CapturedMethod);
        Assert.Equal("/ohvps/obh/s2.0/odeme-emri-rizasi", handler.CapturedRequestUri?.AbsolutePath);
        Assert.Equal("req-004", handler.CapturedHeaders["X-Request-ID"]);
        Assert.Equal("tpp-001", handler.CapturedHeaders["X-Tpp-Code"]);
        Assert.Equal("odeme-riza-1", result.ConsentInfo?.ConsentId);
    }

    [Fact]
    public async Task GetPaymentConsentAsync_WithConsentId_BuildsGetRequestWithConsentIdInPath()
    {
        var handler = new CapturingHttpMessageHandler
        {
            Response = CreateJsonResponse("""
                {
                  "rzBlg": { "rizaNo": "odeme-riza-7", "rizaDrm": "Y" }
                }
                """)
        };
        var client = CreateClient(handler);

        var result = await client.GetPaymentConsentAsync("odeme-riza-7", new Dictionary<string, string>());

        Assert.Equal(HttpMethod.Get, handler.CapturedMethod);
        Assert.Equal("/ohvps/obh/s2.0/odeme-emri-rizasi/odeme-riza-7", handler.CapturedRequestUri?.AbsolutePath);
        Assert.Equal("odeme-riza-7", result.ConsentInfo?.ConsentId);
    }

    [Fact]
    public async Task DeletePaymentConsentAsync_WithConsentId_BuildsDeleteRequest()
    {
        var handler = new CapturingHttpMessageHandler
        {
            Response = new HttpResponseMessage(HttpStatusCode.NoContent)
        };
        var client = CreateClient(handler);
        var headers = new Dictionary<string, string>
        {
            ["X-Request-ID"] = "req-005"
        };

        await client.DeletePaymentConsentAsync("odeme-riza-3", headers);

        Assert.Equal(HttpMethod.Delete, handler.CapturedMethod);
        Assert.Equal("/ohvps/obh/s2.0/odeme-emri-rizasi/odeme-riza-3", handler.CapturedRequestUri?.AbsolutePath);
        Assert.Equal("req-005", handler.CapturedHeaders["X-Request-ID"]);
    }

    [Fact]
    public async Task CreatePaymentOrderAsync_WithRequest_BuildsPostRequestAndMapsResponse()
    {
        var handler = new CapturingHttpMessageHandler
        {
            Response = CreateJsonResponse("""
                {
                  "rzBlg": { "rizaNo": "riza-10", "rizaDrm": "B" },
                  "odmEmr": { "odmEmrNo": "emr-001", "odmDrm": "ISLD" }
                }
                """)
        };
        var client = CreateClient(handler);
        var headers = new Dictionary<string, string>
        {
            ["X-Request-ID"] = "req-006",
            ["X-Aspsp-Code"] = "aspsp-002"
        };
        var request = new YosPaymentDto
        {
            ConsentInfo = new RizaBilgiDto { ConsentId = "riza-10" }
        };

        var result = await client.CreatePaymentOrderAsync(request, headers);

        Assert.Equal(HttpMethod.Post, handler.CapturedMethod);
        Assert.Equal("/ohvps/obh/s2.0/odeme-emri", handler.CapturedRequestUri?.AbsolutePath);
        Assert.Equal("req-006", handler.CapturedHeaders["X-Request-ID"]);
        Assert.Equal("aspsp-002", handler.CapturedHeaders["X-Aspsp-Code"]);
        Assert.Equal("application/json; charset=utf-8", handler.CapturedContentType);
        Assert.Equal("riza-10", result.ConsentInfo?.ConsentId);
        Assert.Equal("emr-001", result.PaymentOrder?.PaymentOrderId);
        Assert.Equal("ISLD", result.PaymentOrder?.PaymentStatus);
    }

    [Fact]
    public async Task GetPaymentOrderAsync_WithPaymentOrderId_BuildsGetRequestWithIdInPath()
    {
        var handler = new CapturingHttpMessageHandler
        {
            Response = CreateJsonResponse("""
                {
                  "odmEmr": { "odmEmrNo": "emr-007", "odmDrm": "BKLD" }
                }
                """)
        };
        var client = CreateClient(handler);

        var result = await client.GetPaymentOrderAsync("emr-007", new Dictionary<string, string>());

        Assert.Equal(HttpMethod.Get, handler.CapturedMethod);
        Assert.Equal("/ohvps/obh/s2.0/odeme-emri/emr-007", handler.CapturedRequestUri?.AbsolutePath);
        Assert.Equal("emr-007", result.PaymentOrder?.PaymentOrderId);
        Assert.Equal("BKLD", result.PaymentOrder?.PaymentStatus);
    }

    [Fact]
    public async Task GetAuthorizationCodeAsync_WithQueryParams_BuildsGetRequestWithQueryString()
    {
        var handler = new CapturingHttpMessageHandler
        {
            Response = CreateJsonResponse("""
                {
                  "bltmKodu": "code-abc",
                  "erisimBelirteci": "token-xyz",
                  "belirtecTur": "Bearer",
                  "gecerlilikSuresi": 3600,
                  "kapsam": "hesap odeme"
                }
                """)
        };
        var client = CreateClient(handler);
        var headers = new Dictionary<string, string>
        {
            ["X-Request-ID"] = "req-007",
            ["X-Aspsp-Code"] = "aspsp-003"
        };

        var result = await client.GetAuthorizationCodeAsync(headers, "state=xyz&rizaNo=riza-1");

        Assert.Equal(HttpMethod.Get, handler.CapturedMethod);
        Assert.Equal("/ohvps/obh/s2.0/gkd/yetki-kodu", handler.CapturedRequestUri?.AbsolutePath);
        Assert.Equal("?state=xyz&rizaNo=riza-1", handler.CapturedRequestUri?.Query);
        Assert.Equal("req-007", handler.CapturedHeaders["X-Request-ID"]);
        Assert.Equal("aspsp-003", handler.CapturedHeaders["X-Aspsp-Code"]);
        Assert.Equal("code-abc", result.AuthorizationCode);
        Assert.Equal("token-xyz", result.AccessToken);
        Assert.Equal("Bearer", result.TokenType);
        Assert.Equal(3600, result.ExpiresIn);
        Assert.Equal("hesap odeme", result.Scope);
    }

    [Fact]
    public async Task GetAuthorizationCodeAsync_WithoutQueryParams_BuildsGetRequestWithoutQueryString()
    {
        var handler = new CapturingHttpMessageHandler
        {
            Response = CreateJsonResponse("""
                {
                  "bltmKodu": "code-def"
                }
                """)
        };
        var client = CreateClient(handler);

        var result = await client.GetAuthorizationCodeAsync(new Dictionary<string, string>());

        Assert.Equal(HttpMethod.Get, handler.CapturedMethod);
        Assert.Equal("/ohvps/obh/s2.0/gkd/yetki-kodu", handler.CapturedRequestUri?.AbsolutePath);
        Assert.Equal("", handler.CapturedRequestUri?.Query);
        Assert.Equal("code-def", result.AuthorizationCode);
    }

    [Fact]
    public async Task CreateAccessTokenAsync_WithRequest_BuildsPostRequestWithHeadersAndMapsResponse()
    {
        var handler = new CapturingHttpMessageHandler
        {
            Response = CreateJsonResponse("""
                {
                  "erisimBelirteci": "token-new",
                  "belirtecTur": "Bearer",
                  "gecerlilikSuresi": 7200,
                  "yenilemeBelireteci": "refresh-new",
                  "kapsam": "hesap"
                }
                """)
        };
        var client = CreateClient(handler);
        var headers = new Dictionary<string, string>
        {
            ["X-Request-ID"] = "req-008",
            ["X-Tpp-Code"] = "tpp-002"
        };
        var request = new YosGkdDto
        {
            AuthorizationCode = "auth-code-99"
        };

        var result = await client.CreateAccessTokenAsync(request, headers);

        Assert.Equal(HttpMethod.Post, handler.CapturedMethod);
        Assert.Equal("/ohvps/obh/s2.0/gkd/erisim-belirteci", handler.CapturedRequestUri?.AbsolutePath);
        Assert.Equal("req-008", handler.CapturedHeaders["X-Request-ID"]);
        Assert.Equal("tpp-002", handler.CapturedHeaders["X-Tpp-Code"]);
        Assert.Equal("application/json; charset=utf-8", handler.CapturedContentType);
        Assert.Contains(""""bltmKodu":"auth-code-99"""", handler.CapturedBody);
        Assert.Equal("token-new", result.AccessToken);
        Assert.Equal("Bearer", result.TokenType);
        Assert.Equal(7200, result.ExpiresIn);
        Assert.Equal("refresh-new", result.RefreshToken);
        Assert.Equal("hesap", result.Scope);
    }

    [Fact]
    public async Task GetPaymentOrderAsync_WithNonSuccessResponse_ThrowsDownstreamServiceException()
    {
        var handler = new CapturingHttpMessageHandler
        {
            Response = new HttpResponseMessage(HttpStatusCode.BadGateway)
            {
                Content = new StringContent("upstream error", Encoding.UTF8, "text/plain")
            }
        };
        var client = CreateClient(handler);

        var exception = await Assert.ThrowsAsync<DownstreamServiceException>(() =>
            client.GetPaymentOrderAsync("emr-fail", new Dictionary<string, string>()));

        Assert.Equal("YosMicroservice", exception.ServiceName);
        Assert.Equal(HttpStatusCode.BadGateway, exception.StatusCode);
        Assert.Equal("upstream error", exception.ErrorMessage);
    }

    private static YosMicroserviceClient CreateClient(CapturingHttpMessageHandler handler) =>
        new(new HttpClient(handler) { BaseAddress = new Uri("https://yos.local") });

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

