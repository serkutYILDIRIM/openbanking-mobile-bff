using openbanking_mobile_bff.Domain.Account.Models.Responses;
using openbanking_mobile_bff.Infrastructure.HttpClients.Hhs;

namespace openbanking_mobile_bff.Domain.Account.Services;

public sealed class AccountService : IAccountService
{
    private readonly IHhsMicroserviceClient _hhsClient;

    public AccountService(IHhsMicroserviceClient hhsClient)
    {
        _hhsClient = hhsClient;
    }

    public Task<AccountListResponse> GetAccountsAsync(string requestId, string aspspCode, string tppCode)
    {
        throw new NotImplementedException();
    }

    public Task<AccountResponse> GetAccountByRefAsync(string accountRef, string requestId, string aspspCode, string tppCode)
    {
        throw new NotImplementedException();
    }

    public Task<BalanceResponse> GetBalanceAsync(string accountRef, string requestId, string aspspCode, string tppCode)
    {
        throw new NotImplementedException();
    }

    public Task<TransactionListResponse> GetTransactionsAsync(string accountRef, string requestId, string aspspCode, string tppCode)
    {
        throw new NotImplementedException();
    }
}

