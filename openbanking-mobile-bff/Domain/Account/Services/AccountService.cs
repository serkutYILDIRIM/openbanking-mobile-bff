using openbanking_mobile_bff.Common.Utilities;
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

    public async Task<AccountListResponse> GetAccountsAsync(string requestId, string aspspCode, string tppCode)
    {
        var headers = HttpHeaderUtil.BuildOhvpsHeaders(requestId, aspspCode, tppCode, jwsSignature: null);
        var dto = await _hhsClient.GetAccountsAsync(headers);
        return new AccountListResponse
        {
            Accounts = new List<AccountResponse>
            {
                new()
                {
                    AccountRef = dto.AccountRef,
                    AccountNumber = dto.AccountNumber,
                    AccountOwner = dto.AccountOwner,
                    AccountType = dto.AccountType,
                    CurrencyCode = dto.CurrencyCode,
                    BranchCode = dto.BranchCode,
                    AccountStatus = dto.AccountStatus
                }
            },
            TotalCount = 1
        };
    }

    public async Task<AccountResponse> GetAccountByRefAsync(string accountRef, string requestId, string aspspCode, string tppCode)
    {
        var headers = HttpHeaderUtil.BuildOhvpsHeaders(requestId, aspspCode, tppCode, jwsSignature: null);
        var dto = await _hhsClient.GetAccountByRefAsync(accountRef, headers);
        return new AccountResponse
        {
            AccountRef = dto.AccountRef,
            AccountNumber = dto.AccountNumber,
            AccountOwner = dto.AccountOwner,
            AccountType = dto.AccountType,
            CurrencyCode = dto.CurrencyCode,
            BranchCode = dto.BranchCode,
            AccountStatus = dto.AccountStatus
        };
    }

    public async Task<BalanceResponse> GetBalanceAsync(string accountRef, string requestId, string aspspCode, string tppCode)
    {
        var headers = HttpHeaderUtil.BuildOhvpsHeaders(requestId, aspspCode, tppCode, jwsSignature: null);
        var dto = await _hhsClient.GetBalanceAsync(accountRef, headers);
        return new BalanceResponse
        {
            AccountRef = dto.AccountRef,
            Amount = dto.Amount,
            CurrencyCode = dto.CurrencyCode,
            BalanceDateTime = dto.BalanceDateTime,
            BalanceType = dto.BalanceType
        };
    }

    public async Task<TransactionListResponse> GetTransactionsAsync(string accountRef, string requestId, string aspspCode, string tppCode)
    {
        var headers = HttpHeaderUtil.BuildOhvpsHeaders(requestId, aspspCode, tppCode, jwsSignature: null);
        var dto = await _hhsClient.GetTransactionsAsync(accountRef, headers);
        return new TransactionListResponse
        {
            Transactions = new List<TransactionItem>
            {
                new()
                {
                    TransactionId = dto.TransactionId,
                    Amount = dto.Amount,
                    CurrencyCode = dto.CurrencyCode,
                    TransactionDate = dto.TransactionDate,
                    Description = dto.Description,
                    CreditorDebitIndicator = dto.CreditorDebitIndicator
                }
            },
            TotalCount = 1
        };
    }
}

