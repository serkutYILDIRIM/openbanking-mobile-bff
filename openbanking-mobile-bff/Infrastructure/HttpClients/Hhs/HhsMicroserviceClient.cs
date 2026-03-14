using openbanking_mobile_bff.Infrastructure.HttpClients.Hhs.Dtos;

namespace openbanking_mobile_bff.Infrastructure.HttpClients.Hhs;

public sealed class HhsMicroserviceClient : IHhsMicroserviceClient
{
    private readonly HttpClient _httpClient;

    public HhsMicroserviceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<HhsAccountDto> GetAccountsAsync(Dictionary<string, string> headers)
    {
        throw new NotImplementedException();
    }

    public Task<HhsAccountDto> GetAccountByRefAsync(string accountRef, Dictionary<string, string> headers)
    {
        throw new NotImplementedException();
    }

    public Task<HhsBalanceDto> GetBalanceAsync(string accountRef, Dictionary<string, string> headers)
    {
        throw new NotImplementedException();
    }

    public Task<HhsTransactionDto> GetTransactionsAsync(string accountRef, Dictionary<string, string> headers)
    {
        throw new NotImplementedException();
    }

    public Task<HhsCardDto> GetCardsAsync(Dictionary<string, string> headers)
    {
        throw new NotImplementedException();
    }

    public Task<HhsCardDto> GetCardByRefAsync(string cardRef, Dictionary<string, string> headers)
    {
        throw new NotImplementedException();
    }

    public Task<HhsCardDto> GetCardDetailAsync(string cardRef, Dictionary<string, string> headers)
    {
        throw new NotImplementedException();
    }

    public Task<HhsCardDto> GetCardTransactionsAsync(string cardRef, Dictionary<string, string> headers)
    {
        throw new NotImplementedException();
    }
}

