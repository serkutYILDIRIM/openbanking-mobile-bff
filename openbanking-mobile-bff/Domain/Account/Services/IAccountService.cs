using openbanking_mobile_bff.Domain.Account.Models.Responses;

namespace openbanking_mobile_bff.Domain.Account.Services;

public interface IAccountService
{
    Task<AccountListResponse> GetAccountsAsync(string requestId, string aspspCode, string tppCode);
    Task<AccountResponse> GetAccountByRefAsync(string accountRef, string requestId, string aspspCode, string tppCode);
    Task<BalanceResponse> GetBalanceAsync(string accountRef, string requestId, string aspspCode, string tppCode);
    Task<TransactionListResponse> GetTransactionsAsync(string accountRef, string requestId, string aspspCode, string tppCode);
}

