using Microsoft.AspNetCore.Mvc;
using openbanking_mobile_bff.Domain.Payment.Models.Requests;
using openbanking_mobile_bff.Domain.Payment.Models.Responses;
using openbanking_mobile_bff.Domain.Payment.Services;

namespace openbanking_mobile_bff.Controllers;

[ApiController]
[Route("api/payments")]
public sealed class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<ActionResult<PaymentResponse>> CreatePaymentOrder(
        [FromBody] PaymentRequest request,
        [FromHeader(Name = "X-Request-ID")] string requestId,
        [FromHeader(Name = "X-ASPSP-Code")] string aspspCode,
        [FromHeader(Name = "X-TPP-Code")] string tppCode)
    {
        var result = await _paymentService.CreatePaymentOrderAsync(request, requestId, aspspCode, tppCode);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentStatusResponse>> GetPaymentOrder(
        string id,
        [FromHeader(Name = "X-Request-ID")] string requestId,
        [FromHeader(Name = "X-ASPSP-Code")] string aspspCode,
        [FromHeader(Name = "X-TPP-Code")] string tppCode)
    {
        var result = await _paymentService.GetPaymentOrderAsync(id, requestId, aspspCode, tppCode);
        return Ok(result);
    }
}

