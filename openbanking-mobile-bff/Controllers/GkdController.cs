using Microsoft.AspNetCore.Mvc;
using openbanking_mobile_bff.Domain.Gkd.Models.Requests;
using openbanking_mobile_bff.Domain.Gkd.Models.Responses;
using openbanking_mobile_bff.Domain.Gkd.Services;

namespace openbanking_mobile_bff.Controllers;

[ApiController]
[Route("api/gkd")]
public sealed class GkdController : ControllerBase
{
    private readonly IGkdProxyService _gkdProxyService;

    public GkdController(IGkdProxyService gkdProxyService)
    {
        _gkdProxyService = gkdProxyService;
    }

    [HttpGet("authorization-code")]
    public async Task<ActionResult<GkdTokenResponse>> GetAuthorizationCode(
        [FromHeader(Name = "X-Request-ID")] string requestId,
        [FromHeader(Name = "X-ASPSP-Code")] string aspspCode,
        [FromHeader(Name = "X-TPP-Code")] string tppCode,
        [FromQuery] string? queryParams = null)
    {
        var result = await _gkdProxyService.GetAuthorizationCodeAsync(requestId, aspspCode, tppCode, queryParams);
        return Ok(result);
    }

    [HttpPost("access-token")]
    public async Task<ActionResult<GkdTokenResponse>> CreateAccessToken(
        [FromBody] GkdTokenRequest request,
        [FromHeader(Name = "X-Request-ID")] string requestId,
        [FromHeader(Name = "X-ASPSP-Code")] string aspspCode,
        [FromHeader(Name = "X-TPP-Code")] string tppCode)
    {
        var result = await _gkdProxyService.CreateAccessTokenAsync(request, requestId, aspspCode, tppCode);
        return Ok(result);
    }
}

