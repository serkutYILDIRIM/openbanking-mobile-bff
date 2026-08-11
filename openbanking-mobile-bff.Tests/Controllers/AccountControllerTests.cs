using Microsoft.AspNetCore.Mvc;
using openbanking_mobile_bff.Controllers;
using openbanking_mobile_bff.Domain.Account.Models.Responses;
using openbanking_mobile_bff.Domain.Account.Services;

namespace openbanking_mobile_bff.Tests.Controllers;

public sealed class AccountControllerTests
{
	[Fact]
	public async Task GetAccounts_ReturnsOkWithResponseAndPassesHeadersToService()
	{
		var expected = new AccountListResponse
		{
			TotalCount = 1,
			Accounts = new List<AccountResponse> { new() { AccountRef = "account-1" } }
		};
		var service = new FakeAccountService { GetAccountsResult = expected };
		var controller = new AccountController(service);

		var actionResult = await controller.GetAccounts("req-123", "aspsp-001", "tpp-001");

		var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
		var value = Assert.IsType<AccountListResponse>(ok.Value);
		Assert.Same(expected, value);
		Assert.Equal(("req-123", "aspsp-001", "tpp-001"), service.GetAccountsArgs);
	}

	[Fact]
	public async Task GetAccountByRef_ReturnsOkWithResponseAndPassesArgumentsToService()
	{
		var expected = new AccountResponse { AccountRef = "account-9" };
		var service = new FakeAccountService { GetAccountByRefResult = expected };
		var controller = new AccountController(service);

		var actionResult = await controller.GetAccountByRef("account-9", "req-123", "aspsp-001", "tpp-001");

		var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
		var value = Assert.IsType<AccountResponse>(ok.Value);
		Assert.Same(expected, value);
		Assert.Equal(("account-9", "req-123", "aspsp-001", "tpp-001"), service.GetAccountByRefArgs);
	}

	[Fact]
	public async Task GetBalance_ReturnsOkWithResponseAndPassesArgumentsToService()
	{
		var expected = new BalanceResponse { AccountRef = "account-7" };
		var service = new FakeAccountService { GetBalanceResult = expected };
		var controller = new AccountController(service);

		var actionResult = await controller.GetBalance("account-7", "req-123", "aspsp-001", "tpp-001");

		var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
		var value = Assert.IsType<BalanceResponse>(ok.Value);
		Assert.Same(expected, value);
		Assert.Equal(("account-7", "req-123", "aspsp-001", "tpp-001"), service.GetBalanceArgs);
	}

	private sealed class FakeAccountService : IAccountService
	{
		public AccountListResponse GetAccountsResult { get; set; } = new();
		public AccountResponse GetAccountByRefResult { get; set; } = new();
		public BalanceResponse GetBalanceResult { get; set; } = new();
		public TransactionListResponse GetTransactionsResult { get; set; } = new();

		public (string RequestId, string AspspCode, string TppCode)? GetAccountsArgs { get; private set; }
		public (string AccountRef, string RequestId, string AspspCode, string TppCode)? GetAccountByRefArgs { get; private set; }
		public (string AccountRef, string RequestId, string AspspCode, string TppCode)? GetBalanceArgs { get; private set; }
		public (string AccountRef, string RequestId, string AspspCode, string TppCode)? GetTransactionsArgs { get; private set; }

		public Task<AccountListResponse> GetAccountsAsync(string requestId, string aspspCode, string tppCode)
		{
			GetAccountsArgs = (requestId, aspspCode, tppCode);
			return Task.FromResult(GetAccountsResult);
		}

		public Task<AccountResponse> GetAccountByRefAsync(string accountRef, string requestId, string aspspCode, string tppCode)
		{
			GetAccountByRefArgs = (accountRef, requestId, aspspCode, tppCode);
			return Task.FromResult(GetAccountByRefResult);
		}

		public Task<BalanceResponse> GetBalanceAsync(string accountRef, string requestId, string aspspCode, string tppCode)
		{
			GetBalanceArgs = (accountRef, requestId, aspspCode, tppCode);
			return Task.FromResult(GetBalanceResult);
		}

		public Task<TransactionListResponse> GetTransactionsAsync(string accountRef, string requestId, string aspspCode, string tppCode)
		{
			GetTransactionsArgs = (accountRef, requestId, aspspCode, tppCode);
			return Task.FromResult(GetTransactionsResult);
		}
	}
}

