using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using openbanking_mobile_bff.Common.Exceptions;
using openbanking_mobile_bff.Infrastructure.HttpClients.Yos.Dtos;

namespace openbanking_mobile_bff.Infrastructure.HttpClients.Yos;

public sealed class YosMicroserviceClient : IYosMicroserviceClient
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public YosMicroserviceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<YosConsentDto> CreateAccountConsentAsync(YosConsentDto request, Dictionary<string, string> headers)
    {
        var message = BuildRequest(HttpMethod.Post, "/ohvps/obh/s2.0/hesap-bilgisi-rizasi", headers, request);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<YosConsentDto>(response, "YosMicroservice");
    }

    public async Task<YosConsentDto> GetAccountConsentAsync(string consentId, Dictionary<string, string> headers)
    {
        var message = BuildRequest(HttpMethod.Get, $"/ohvps/obh/s2.0/hesap-bilgisi-rizasi/{consentId}", headers);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<YosConsentDto>(response, "YosMicroservice");
    }

    public async Task DeleteAccountConsentAsync(string consentId, Dictionary<string, string> headers)
    {
        var message = BuildRequest(HttpMethod.Delete, $"/ohvps/obh/s2.0/hesap-bilgisi-rizasi/{consentId}", headers);
        var response = await _httpClient.SendAsync(message);
        await EnsureSuccessAsync(response, "YosMicroservice");
    }

    public async Task<YosConsentDto> CreatePaymentConsentAsync(YosConsentDto request, Dictionary<string, string> headers)
    {
        var message = BuildRequest(HttpMethod.Post, "/ohvps/obh/s2.0/odeme-emri-rizasi", headers, request);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<YosConsentDto>(response, "YosMicroservice");
    }

    public async Task<YosConsentDto> GetPaymentConsentAsync(string consentId, Dictionary<string, string> headers)
    {
        var message = BuildRequest(HttpMethod.Get, $"/ohvps/obh/s2.0/odeme-emri-rizasi/{consentId}", headers);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<YosConsentDto>(response, "YosMicroservice");
    }

    public async Task DeletePaymentConsentAsync(string consentId, Dictionary<string, string> headers)
    {
        var message = BuildRequest(HttpMethod.Delete, $"/ohvps/obh/s2.0/odeme-emri-rizasi/{consentId}", headers);
        var response = await _httpClient.SendAsync(message);
        await EnsureSuccessAsync(response, "YosMicroservice");
    }

    public async Task<YosPaymentDto> CreatePaymentOrderAsync(YosPaymentDto request, Dictionary<string, string> headers)
    {
        var message = BuildRequest(HttpMethod.Post, "/ohvps/obh/s2.0/odeme-emri", headers, request);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<YosPaymentDto>(response, "YosMicroservice");
    }

    public async Task<YosPaymentDto> GetPaymentOrderAsync(string paymentOrderId, Dictionary<string, string> headers)
    {
        var message = BuildRequest(HttpMethod.Get, $"/ohvps/obh/s2.0/odeme-emri/{paymentOrderId}", headers);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<YosPaymentDto>(response, "YosMicroservice");
    }

    public async Task<YosGkdDto> GetAuthorizationCodeAsync(Dictionary<string, string> headers, string? queryParams = null)
    {
        var path = string.IsNullOrEmpty(queryParams)
            ? "/ohvps/obh/s2.0/gkd/yetki-kodu"
            : $"/ohvps/obh/s2.0/gkd/yetki-kodu?{queryParams}";
        var message = BuildRequest(HttpMethod.Get, path, headers);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<YosGkdDto>(response, "YosMicroservice");
    }

    public async Task<YosGkdDto> CreateAccessTokenAsync(YosGkdDto request, Dictionary<string, string> headers)
    {
        var message = BuildRequest(HttpMethod.Post, "/ohvps/obh/s2.0/gkd/erisim-belirteci", headers, request);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<YosGkdDto>(response, "YosMicroservice");
    }

    private static HttpRequestMessage BuildRequest<T>(HttpMethod method, string path, Dictionary<string, string> headers, T? body = default)
    {
        var message = new HttpRequestMessage(method, path);
        foreach (var (key, value) in headers)
            message.Headers.TryAddWithoutValidation(key, value);

        if (body is not null)
            message.Content = JsonContent.Create(body, options: JsonOptions);

        return message;
    }

    private static HttpRequestMessage BuildRequest(HttpMethod method, string path, Dictionary<string, string> headers)
    {
        var message = new HttpRequestMessage(method, path);
        foreach (var (key, value) in headers)
            message.Headers.TryAddWithoutValidation(key, value);
        return message;
    }

    private static async Task<T> DeserializeAsync<T>(HttpResponseMessage response, string serviceName)
    {
        await EnsureSuccessAsync(response, serviceName);
        var result = await response.Content.ReadFromJsonAsync<T>(JsonOptions);
        return result!;
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, string serviceName)
    {
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new DownstreamServiceException(serviceName, body, response.StatusCode);
        }
    }
}

