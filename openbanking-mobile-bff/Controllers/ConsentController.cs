using Microsoft.AspNetCore.Mvc;
using openbanking_mobile_bff.Domain.Consent.Models.Requests;
using openbanking_mobile_bff.Domain.Consent.Models.Responses;
using openbanking_mobile_bff.Domain.Consent.Services;

namespace openbanking_mobile_bff.Controllers;

[ApiController]
[Route("api/consent")]
public sealed class ConsentController : ControllerBase
{
    private readonly IConsentService _consentService;

    public ConsentController(IConsentService consentService)
    {
        _consentService = consentService;
    }

    [HttpPost("account-info-consent")]
    public async Task<ActionResult<ConsentResponse>> CreateAccountConsent(
        [FromBody] ConsentRequest request,
        [FromHeader(Name = "X-Request-ID")] string requestId,
        [FromHeader(Name = "X-ASPSP-Code")] string aspspCode,
        [FromHeader(Name = "X-TPP-Code")] string tppCode)
    {
        var result = await _consentService.CreateAccountConsentAsync(request, requestId, aspspCode, tppCode);
        return Ok(result);
    }

    [HttpGet("account-info-consent/{id}")]
    public async Task<ActionResult<ConsentResponse>> GetAccountConsent(
        string id,
        [FromHeader(Name = "X-Request-ID")] string requestId,
        [FromHeader(Name = "X-ASPSP-Code")] string aspspCode,
        [FromHeader(Name = "X-TPP-Code")] string tppCode)
    {
        var result = await _consentService.GetAccountConsentAsync(id, requestId, aspspCode, tppCode);
        return Ok(result);
    }

    [HttpDelete("account-info-consent/{id}")]
    public async Task<IActionResult> DeleteAccountConsent(
        string id,
        [FromHeader(Name = "X-Request-ID")] string requestId,
        [FromHeader(Name = "X-ASPSP-Code")] string aspspCode,
        [FromHeader(Name = "X-TPP-Code")] string tppCode)
    {
        await _consentService.DeleteAccountConsentAsync(id, requestId, aspspCode, tppCode);
        return NoContent();
    }

    [HttpPost("payment-order-consent")]
    public async Task<ActionResult<ConsentResponse>> CreatePaymentConsent(
        [FromBody] ConsentRequest request,
        [FromHeader(Name = "X-Request-ID")] string requestId,
        [FromHeader(Name = "X-ASPSP-Code")] string aspspCode,
        [FromHeader(Name = "X-TPP-Code")] string tppCode)
    {
        var result = await _consentService.CreatePaymentConsentAsync(request, requestId, aspspCode, tppCode);
        return Ok(result);
    }

    [HttpGet("payment-order-consent/{id}")]
    public async Task<ActionResult<ConsentResponse>> GetPaymentConsent(
        string id,
        [FromHeader(Name = "X-Request-ID")] string requestId,
        [FromHeader(Name = "X-ASPSP-Code")] string aspspCode,
        [FromHeader(Name = "X-TPP-Code")] string tppCode)
    {
        var result = await _consentService.GetPaymentConsentAsync(id, requestId, aspspCode, tppCode);
        return Ok(result);
    }

    [HttpDelete("payment-order-consent/{id}")]
    public async Task<IActionResult> DeletePaymentConsent(
        string id,
        [FromHeader(Name = "X-Request-ID")] string requestId,
        [FromHeader(Name = "X-ASPSP-Code")] string aspspCode,
        [FromHeader(Name = "X-TPP-Code")] string tppCode)
    {
        await _consentService.DeletePaymentConsentAsync(id, requestId, aspspCode, tppCode);
        return NoContent();
    }
}

