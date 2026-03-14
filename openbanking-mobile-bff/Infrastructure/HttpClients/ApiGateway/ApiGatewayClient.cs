using System.Net.Http.Json;
using System.Text.Json;
using openbanking_mobile_bff.Common.Exceptions;
using openbanking_mobile_bff.Infrastructure.HttpClients.ApiGateway.Dtos;

namespace openbanking_mobile_bff.Infrastructure.HttpClients.ApiGateway;

public sealed class ApiGatewayClient : IApiGatewayClient
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ApiGatewayClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GatewayAccessTokenDto> GetAuthorizationCodeAsync(Dictionary<string, string> headers, string? queryParams = null)
    {
        var path = string.IsNullOrEmpty(queryParams)
            ? "/ohvps/gw/s2.0/gkd/yetki-kodu"
            : $"/ohvps/gw/s2.0/gkd/yetki-kodu?{queryParams}";

        var message = BuildRequest(HttpMethod.Get, path, headers);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<GatewayAccessTokenDto>(response, "ApiGateway");
    }

    public async Task<GatewayAccessTokenDto> CreateAccessTokenAsync(GatewayAuthCodeDto request, Dictionary<string, string> headers)
    {
        var message = BuildRequest(HttpMethod.Post, "/ohvps/gw/s2.0/gkd/erisim-belirteci", headers, request);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<GatewayAccessTokenDto>(response, "ApiGateway");
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
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new DownstreamServiceException(serviceName, body, response.StatusCode);
        }

        var result = await response.Content.ReadFromJsonAsync<T>(JsonOptions);
        return result!;
    }
}

