﻿using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using openbanking_mobile_bff.Common.Exceptions;
using openbanking_mobile_bff.Configuration;
using openbanking_mobile_bff.Infrastructure.HttpClients.Hhs.Dtos;

namespace openbanking_mobile_bff.Infrastructure.HttpClients.Hhs;

public sealed class HhsMicroserviceClient : IHhsMicroserviceClient
{
    private readonly HttpClient _httpClient;
    private readonly HhsApiPathOptions _paths;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public HhsMicroserviceClient(HttpClient httpClient, IOptions<HhsApiPathOptions> pathOptions)
    {
        _httpClient = httpClient;
        _paths = pathOptions.Value;
    }

    public async Task<HhsAccountDto> GetAccountsAsync(Dictionary<string, string> headers)
    {
        var message = BuildRequest(HttpMethod.Get, _paths.AccountsPath, headers);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<HhsAccountDto>(response);
    }

    public async Task<HhsAccountDto> GetAccountByRefAsync(string accountRef, Dictionary<string, string> headers)
    {
        var path = _paths.AccountByRefPath.Replace("{accountRef}", accountRef);
        var message = BuildRequest(HttpMethod.Get, path, headers);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<HhsAccountDto>(response);
    }

    public async Task<HhsBalanceDto> GetBalanceAsync(string accountRef, Dictionary<string, string> headers)
    {
        var path = _paths.BalancePath.Replace("{accountRef}", accountRef);
        var message = BuildRequest(HttpMethod.Get, path, headers);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<HhsBalanceDto>(response);
    }

    public async Task<HhsTransactionDto> GetTransactionsAsync(string accountRef, Dictionary<string, string> headers)
    {
        var path = _paths.TransactionsPath.Replace("{accountRef}", accountRef);
        var message = BuildRequest(HttpMethod.Get, path, headers);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<HhsTransactionDto>(response);
    }

    public async Task<HhsCardDto> GetCardsAsync(Dictionary<string, string> headers)
    {
        var message = BuildRequest(HttpMethod.Get, _paths.CardsPath, headers);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<HhsCardDto>(response);
    }

    public async Task<HhsCardDto> GetCardByRefAsync(string cardRef, Dictionary<string, string> headers)
    {
        var path = _paths.CardByRefPath.Replace("{cardRef}", cardRef);
        var message = BuildRequest(HttpMethod.Get, path, headers);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<HhsCardDto>(response);
    }

    public async Task<HhsCardDto> GetCardDetailAsync(string cardRef, Dictionary<string, string> headers)
    {
        var path = _paths.CardDetailPath.Replace("{cardRef}", cardRef);
        var message = BuildRequest(HttpMethod.Get, path, headers);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<HhsCardDto>(response);
    }

    public async Task<HhsCardDto> GetCardTransactionsAsync(string cardRef, Dictionary<string, string> headers)
    {
        var path = _paths.CardTransactionsPath.Replace("{cardRef}", cardRef);
        var message = BuildRequest(HttpMethod.Get, path, headers);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<HhsCardDto>(response);
    }

    public async Task<HhsPaymentDto> CreatePaymentOrderAsync(HhsPaymentDto request, Dictionary<string, string> headers)
    {
        var message = BuildRequest(HttpMethod.Post, _paths.PaymentsPath, headers, request);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<HhsPaymentDto>(response);
    }

    public async Task<HhsPaymentDto> GetPaymentOrderAsync(string paymentOrderId, Dictionary<string, string> headers)
    {
        var path = _paths.PaymentByIdPath.Replace("{paymentOrderId}", paymentOrderId);
        var message = BuildRequest(HttpMethod.Get, path, headers);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<HhsPaymentDto>(response);
    }

    public async Task<object?> LinkAccountAsync(object request, Dictionary<string, string> headers)
    {
        if (string.IsNullOrWhiteSpace(_paths.AccountLinkPath))
            throw new NotImplementedException("AccountLinkPath is not configured.");

        var message = BuildRequest(HttpMethod.Post, _paths.AccountLinkPath, headers, request);
        var response = await _httpClient.SendAsync(message);
        return await DeserializeAsync<JsonElement>(response);
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

    private static async Task<T> DeserializeAsync<T>(HttpResponseMessage response)
    {
        await EnsureSuccessAsync(response);
        var result = await response.Content.ReadFromJsonAsync<T>(JsonOptions);
        return result!;
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new DownstreamServiceException("HhsMicroservice", body, response.StatusCode);
        }
    }
}

