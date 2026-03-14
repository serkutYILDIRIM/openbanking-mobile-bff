using Microsoft.AspNetCore.Mvc;
using openbanking_mobile_bff.Domain.Card.Models.Responses;
using openbanking_mobile_bff.Domain.Card.Services;

namespace openbanking_mobile_bff.Controllers;

[ApiController]
[Route("api/cards")]
public sealed class CardController : ControllerBase
{
    private readonly ICardService _cardService;

    public CardController(ICardService cardService)
    {
        _cardService = cardService;
    }

    [HttpGet]
    public async Task<ActionResult<CardListResponse>> GetCards(
        [FromHeader(Name = "X-Request-ID")] string requestId,
        [FromHeader(Name = "X-ASPSP-Code")] string aspspCode,
        [FromHeader(Name = "X-TPP-Code")] string tppCode)
    {
        var result = await _cardService.GetCardsAsync(requestId, aspspCode, tppCode);
        return Ok(result);
    }

    [HttpGet("{cardRef}")]
    public async Task<ActionResult<CardResponse>> GetCardByRef(
        string cardRef,
        [FromHeader(Name = "X-Request-ID")] string requestId,
        [FromHeader(Name = "X-ASPSP-Code")] string aspspCode,
        [FromHeader(Name = "X-TPP-Code")] string tppCode)
    {
        var result = await _cardService.GetCardByRefAsync(cardRef, requestId, aspspCode, tppCode);
        return Ok(result);
    }

    [HttpGet("{cardRef}/detail")]
    public async Task<ActionResult<CardResponse>> GetCardDetail(
        string cardRef,
        [FromHeader(Name = "X-Request-ID")] string requestId,
        [FromHeader(Name = "X-ASPSP-Code")] string aspspCode,
        [FromHeader(Name = "X-TPP-Code")] string tppCode)
    {
        var result = await _cardService.GetCardDetailAsync(cardRef, requestId, aspspCode, tppCode);
        return Ok(result);
    }

    [HttpGet("{cardRef}/transactions")]
    public async Task<ActionResult<CardTransactionResponse>> GetCardTransactions(
        string cardRef,
        [FromHeader(Name = "X-Request-ID")] string requestId,
        [FromHeader(Name = "X-ASPSP-Code")] string aspspCode,
        [FromHeader(Name = "X-TPP-Code")] string tppCode)
    {
        var result = await _cardService.GetCardTransactionsAsync(cardRef, requestId, aspspCode, tppCode);
        return Ok(result);
    }
}

