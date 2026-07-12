using openbanking_mobile_bff.Common.Constants;
using openbanking_mobile_bff.Domain.Account.Services;
using openbanking_mobile_bff.Infrastructure.HttpClients.Hhs;
using openbanking_mobile_bff.Infrastructure.HttpClients.Hhs.Dtos;

namespace openbanking_mobile_bff.Tests.Domain.Account.Services;

public sealed class AccountServiceTests
{
    [Fact]
    public async Task GetAccountsAsync_WithAccountFromClient_MapsDtoIntoSingleAccountResponse()
    {
        var dto = new HhsAccountDto
        {
            AccountRef = "acc-ref-1",
            AccountNumber = "1234567890",
            AccountOwner = "Jane Doe",
            AccountType = "current",
            CurrencyCode = "TRY",
            BranchCode = "0001",
            AccountStatus = "active"
        };
        var client = new FakeHhsMicroserviceClient { AccountResult = dto };
        var service = new AccountService(client);

        var result = await service.GetAccountsAsync("req-123", "aspsp-001", "tpp-001");

        Assert.Equal(1, result.TotalCount);
        
        var account = Assert.Single(result.Accounts);
        Assert.Equal("acc-ref-1", account.AccountRef);
        Assert.Equal("1234567890", account.AccountNumber);
        Assert.Equal("Jane Doe", account.AccountOwner);
        Assert.Equal("current", account.AccountType);
        Assert.Equal("TRY", account.CurrencyCode);
        Assert.Equal("0001", account.BranchCode);
        Assert.Equal("active", account.AccountStatus);
    }

    [Fact]
    public async Task GetAccountsAsync_BuildsOhvpsHeadersWithoutJwsSignature()
    {
        var client = new FakeHhsMicroserviceClient { AccountResult = new HhsAccountDto() };
        var service = new AccountService(client);

        await service.GetAccountsAsync("req-123", "aspsp-001", "tpp-001");

        Assert.NotNull(client.CapturedHeaders);
        Assert.Equal(3, client.CapturedHeaders!.Count);
        Assert.Equal("req-123", client.CapturedHeaders[OhvpsConstants.RequestIdHeader]);
        Assert.Equal("aspsp-001", client.CapturedHeaders[OhvpsConstants.AspspCodeHeader]);
        Assert.Equal("tpp-001", client.CapturedHeaders[OhvpsConstants.TppCodeHeader]);
        Assert.False(client.CapturedHeaders.ContainsKey(OhvpsConstants.JwsSignatureHeader));
    }

    [Fact]
    public async Task GetAccountByRefAsync_PassesAccountRefToClientAndMapsResponse()
    {
        var dto = new HhsAccountDto
        {
            AccountRef = "acc-ref-9",
            AccountNumber = "9876543210",
            AccountOwner = "John Smith",
            AccountType = "savings",
            CurrencyCode = "USD",
            BranchCode = "0002",
            AccountStatus = "passive"
        };
        
        var client = new FakeHhsMicroserviceClient { AccountResult = dto };
        var service = new AccountService(client);

        var result = await service.GetAccountByRefAsync("acc-ref-9", "req-123", "aspsp-001", "tpp-001");

        Assert.Equal("acc-ref-9", client.CapturedAccountRef);
        Assert.Equal("acc-ref-9", result.AccountRef);
        Assert.Equal("9876543210", result.AccountNumber);
        Assert.Equal("John Smith", result.AccountOwner);
        Assert.Equal("savings", result.AccountType);
        Assert.Equal("USD", result.CurrencyCode);
        Assert.Equal("0002", result.BranchCode);
        Assert.Equal("passive", result.AccountStatus);
    }

    [Fact]
    public async Task GetBalanceAsync_MapsDtoIntoBalanceResponse()
    {
        var balanceDateTime = new DateTime(2024, 1, 15, 10, 30, 45, DateTimeKind.Utc);
        
        var dto = new HhsBalanceDto
        {
            AccountRef = "acc-ref-1",
            Amount = "1500.75",
            CurrencyCode = "TRY",
            BalanceDateTime = balanceDateTime,
            BalanceType = "available"
        };
        
        var client = new FakeHhsMicroserviceClient { BalanceResult = dto };
        var service = new AccountService(client);

        var result = await service.GetBalanceAsync("acc-ref-1", "req-123", "aspsp-001", "tpp-001");

        Assert.Equal("acc-ref-1", client.CapturedAccountRef);
        Assert.Equal("acc-ref-1", result.AccountRef);
        Assert.Equal("1500.75", result.Amount);
        Assert.Equal("TRY", result.CurrencyCode);
        Assert.Equal(balanceDateTime, result.BalanceDateTime);
        Assert.Equal("available", result.BalanceType);
    }

    [Fact]
    public async Task GetTransactionsAsync_MapsDtoIntoSingleTransactionItem()
    {
        var transactionDate = new DateTime(2024, 2, 20, 8, 15, 0, DateTimeKind.Utc);
        var dto = new HhsTransactionDto
        {
            TransactionId = "txn-1",
            Amount = "250.00",
            CurrencyCode = "EUR",
            TransactionDate = transactionDate,
            Description = "Grocery store",
            CreditorDebitIndicator = "DBIT"
        };
        var client = new FakeHhsMicroserviceClient { TransactionResult = dto };
        var service = new AccountService(client);

        var result = await service.GetTransactionsAsync("acc-ref-1", "req-123", "aspsp-001", "tpp-001");

        Assert.Equal("acc-ref-1", client.CapturedAccountRef);
        Assert.Equal(1, result.TotalCount);
        var transaction = Assert.Single(result.Transactions);
        Assert.Equal("txn-1", transaction.TransactionId);
        Assert.Equal("250.00", transaction.Amount);
        Assert.Equal("EUR", transaction.CurrencyCode);
        Assert.Equal(transactionDate, transaction.TransactionDate);
        Assert.Equal("Grocery store", transaction.Description);
        Assert.Equal("DBIT", transaction.CreditorDebitIndicator);
    }

    private sealed class FakeHhsMicroserviceClient : IHhsMicroserviceClient
    {
        public HhsAccountDto AccountResult { get; set; } = new();
        public HhsBalanceDto BalanceResult { get; set; } = new();
        public HhsTransactionDto TransactionResult { get; set; } = new();

        public Dictionary<string, string>? CapturedHeaders { get; private set; }
        public string? CapturedAccountRef { get; private set; }

        public Task<HhsAccountDto> GetAccountsAsync(Dictionary<string, string> headers)
        {
            CapturedHeaders = headers;
            return Task.FromResult(AccountResult);
        }

        public Task<HhsAccountDto> GetAccountByRefAsync(string accountRef, Dictionary<string, string> headers)
        {
            CapturedAccountRef = accountRef;
            CapturedHeaders = headers;
            return Task.FromResult(AccountResult);
        }

        public Task<HhsBalanceDto> GetBalanceAsync(string accountRef, Dictionary<string, string> headers)
        {
            CapturedAccountRef = accountRef;
            CapturedHeaders = headers;
            return Task.FromResult(BalanceResult);
        }

        public Task<HhsTransactionDto> GetTransactionsAsync(string accountRef, Dictionary<string, string> headers)
        {
            CapturedAccountRef = accountRef;
            CapturedHeaders = headers;
            return Task.FromResult(TransactionResult);
        }

        public Task<HhsCardDto> GetCardsAsync(Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<HhsCardDto> GetCardByRefAsync(string cardRef, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<HhsCardDto> GetCardDetailAsync(string cardRef, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<HhsCardDto> GetCardTransactionsAsync(string cardRef, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<HhsPaymentDto> CreatePaymentOrderAsync(HhsPaymentDto request, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<HhsPaymentDto> GetPaymentOrderAsync(string paymentOrderId, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<object?> LinkAccountAsync(object request, Dictionary<string, string> headers) =>
            throw new NotImplementedException();
    }
}
