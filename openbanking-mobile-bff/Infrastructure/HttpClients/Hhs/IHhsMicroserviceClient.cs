using openbanking_mobile_bff.Infrastructure.HttpClients.Hhs.Dtos;

namespace openbanking_mobile_bff.Infrastructure.HttpClients.Hhs;

public interface IHhsMicroserviceClient
{
    Task<HhsAccountDto> GetAccountsAsync(Dictionary<string, string> headers);
    Task<HhsAccountDto> GetAccountByRefAsync(string accountRef, Dictionary<string, string> headers);
    Task<HhsBalanceDto> GetBalanceAsync(string accountRef, Dictionary<string, string> headers);
    Task<HhsTransactionDto> GetTransactionsAsync(string accountRef, Dictionary<string, string> headers);
    Task<HhsCardDto> GetCardsAsync(Dictionary<string, string> headers);
    Task<HhsCardDto> GetCardByRefAsync(string cardRef, Dictionary<string, string> headers);
    Task<HhsCardDto> GetCardDetailAsync(string cardRef, Dictionary<string, string> headers);
    Task<HhsCardDto> GetCardTransactionsAsync(string cardRef, Dictionary<string, string> headers);
}

