using System.Net;
using System.Text;
using Microsoft.Extensions.Options;
using openbanking_mobile_bff.Common.Exceptions;
using openbanking_mobile_bff.Configuration;
using openbanking_mobile_bff.Infrastructure.HttpClients.Hhs;
using openbanking_mobile_bff.Infrastructure.HttpClients.Hhs.Dtos;

namespace openbanking_mobile_bff.Tests.Infrastructure.HttpClients.Hhs;

public sealed class HhsMicroserviceClientTests
{
    [Fact]
    public async Task GetAccountByRefAsync_WithAccountRef_BuildsGetRequestWithHeadersAndMapsResponse()
    {
        var handler = new CapturingHttpMessageHandler
        {
            Response = CreateJsonResponse("""
                {
                  "hspRef": "acc-123",
                  "hspNo": "TR001",
                  "hspSahibi": "Jane Doe"
                }
                """)
        };
        var client = CreateClient(handler, new HhsApiPathOptions
        {
            AccountByRefPath = "/accounts/{accountRef}"
        });
        var headers = new Dictionary<string, string>
        {
            ["X-Request-ID"] = "req-123",
            ["X-TPP-Code"] = "tpp-001"
        };

        var result = await client.GetAccountByRefAsync("acc-123", headers);

        Assert.Equal(HttpMethod.Get, handler.CapturedMethod);
        Assert.Equal("/accounts/acc-123", handler.CapturedRequestUri?.AbsolutePath);
        Assert.Equal("req-123", handler.CapturedHeaders["X-Request-ID"]);
        Assert.Equal("tpp-001", handler.CapturedHeaders["X-TPP-Code"]);
        Assert.Equal("acc-123", result.AccountRef);
        Assert.Equal("TR001", result.AccountNumber);
        Assert.Equal("Jane Doe", result.AccountOwner);
    }

    [Fact]
    public async Task CreatePaymentOrderAsync_WithRequest_BuildsPostRequestWithSerializedBody()
    {
        var handler = new CapturingHttpMessageHandler
        {
            Response = CreateJsonResponse("""
                {
                  "odmEmriNo": "pay-1",
                  "odmDrm": "BKLD"
                }
                """)
        };
        var client = CreateClient(handler, new HhsApiPathOptions
        {
            PaymentsPath = "/payments"
        });
        var headers = new Dictionary<string, string>
        {
            ["X-Request-ID"] = "req-456"
        };
        var request = new HhsPaymentDto
        {
            PaymentOrderId = "pay-1",
            PaymentStatus = "HAZR"
        };

        var result = await client.CreatePaymentOrderAsync(request, headers);

        Assert.Equal(HttpMethod.Post, handler.CapturedMethod);
        Assert.Equal("/payments", handler.CapturedRequestUri?.AbsolutePath);
        Assert.Equal("req-456", handler.CapturedHeaders["X-Request-ID"]);
        Assert.Equal("application/json; charset=utf-8", handler.CapturedContentType);
        Assert.Equal("""{"odmEmriNo":"pay-1","odmDrm":"HAZR","odmTtr":null,"prBrm":null,"gndAd":null,"gndHspNo":null,"alcAd":null,"alcHspNo":null,"odmAcklm":null,"odmZmn":null}""", handler.CapturedBody);
        Assert.Equal("pay-1", result.PaymentOrderId);
        Assert.Equal("BKLD", result.PaymentStatus);
    }

    [Fact]
    public async Task GetPaymentOrderAsync_WithNonSuccessResponse_ThrowsDownstreamServiceException()
    {
        var handler = new CapturingHttpMessageHandler
        {
            Response = new HttpResponseMessage(HttpStatusCode.BadGateway)
            {
                Content = new StringContent("hhs failed", Encoding.UTF8, "text/plain")
            }
        };
        var client = CreateClient(handler, new HhsApiPathOptions
        {
            PaymentByIdPath = "/payments/{paymentOrderId}"
        });

        var exception = await Assert.ThrowsAsync<DownstreamServiceException>(() =>
            client.GetPaymentOrderAsync("pay-fail", new Dictionary<string, string>()));

        Assert.Equal("HhsMicroservice", exception.ServiceName);
        Assert.Equal(HttpStatusCode.BadGateway, exception.StatusCode);
        Assert.Equal("hhs failed", exception.ErrorMessage);
    }

    [Fact]
    public async Task LinkAccountAsync_WithoutConfiguredPath_ThrowsNotImplementedException()
    {
        var client = CreateClient(new CapturingHttpMessageHandler(), new HhsApiPathOptions
        {
            AccountLinkPath = " "
        });

        var exception = await Assert.ThrowsAsync<NotImplementedException>(() =>
            client.LinkAccountAsync(new { accountRef = "acc-1" }, new Dictionary<string, string>()));

        Assert.Equal("AccountLinkPath is not configured.", exception.Message);
    }

    private static HhsMicroserviceClient CreateClient(CapturingHttpMessageHandler handler, HhsApiPathOptions options) =>
        new(
            new HttpClient(handler) { BaseAddress = new Uri("https://hhs.local") },
            Options.Create(options));

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

