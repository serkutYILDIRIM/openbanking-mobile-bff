using Microsoft.AspNetCore.Mvc;
using openbanking_mobile_bff.Domain.Account.Models.Responses;
using openbanking_mobile_bff.Domain.Account.Services;

namespace openbanking_mobile_bff.Controllers;

[ApiController]
[Route("api/accounts")]
public sealed class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<ActionResult<AccountListResponse>> GetAccounts(
        [FromHeader(Name = "X-Request-ID")] string requestId,
        [FromHeader(Name = "X-ASPSP-Code")] string aspspCode,
        [FromHeader(Name = "X-TPP-Code")] string tppCode)
    {
        var result = await _accountService.GetAccountsAsync(requestId, aspspCode, tppCode);
        return Ok(result);
    }

    [HttpGet("{accountRef}")]
    public async Task<ActionResult<AccountResponse>> GetAccountByRef(
        string accountRef,
        [FromHeader(Name = "X-Request-ID")] string requestId,
        [FromHeader(Name = "X-ASPSP-Code")] string aspspCode,
        [FromHeader(Name = "X-TPP-Code")] string tppCode)
    {
        var result = await _accountService.GetAccountByRefAsync(accountRef, requestId, aspspCode, tppCode);
        return Ok(result);
    }

    [HttpGet("{accountRef}/balance")]
    public async Task<ActionResult<BalanceResponse>> GetBalance(
        string accountRef,
        [FromHeader(Name = "X-Request-ID")] string requestId,
        [FromHeader(Name = "X-ASPSP-Code")] string aspspCode,
        [FromHeader(Name = "X-TPP-Code")] string tppCode)
    {
        var result = await _accountService.GetBalanceAsync(accountRef, requestId, aspspCode, tppCode);
        return Ok(result);
    }

    [HttpGet("{accountRef}/transactions")]
    public async Task<ActionResult<TransactionListResponse>> GetTransactions(
        string accountRef,
        [FromHeader(Name = "X-Request-ID")] string requestId,
        [FromHeader(Name = "X-ASPSP-Code")] string aspspCode,
        [FromHeader(Name = "X-TPP-Code")] string tppCode)
    {
        var result = await _accountService.GetTransactionsAsync(accountRef, requestId, aspspCode, tppCode);
        return Ok(result);
    }
}

