using openbanking_mobile_bff.Common.Utilities;
using openbanking_mobile_bff.Domain.Card.Models.Responses;
using openbanking_mobile_bff.Infrastructure.HttpClients.Hhs;

namespace openbanking_mobile_bff.Domain.Card.Services;

public sealed class CardService : ICardService
{
    private readonly IHhsMicroserviceClient _hhsClient;

    public CardService(IHhsMicroserviceClient hhsClient)
    {
        _hhsClient = hhsClient;
    }

    public async Task<CardListResponse> GetCardsAsync(string requestId, string aspspCode, string tppCode)
    {
        var headers = HttpHeaderUtil.BuildOhvpsHeaders(requestId, aspspCode, tppCode, jwsSignature: null);
        var dto = await _hhsClient.GetCardsAsync(headers);
        return new CardListResponse
        {
            Cards = new List<CardResponse>
            {
                new()
                {
                    CardRef = dto.CardRef,
                    CardNumber = dto.CardNumber,
                    CardHolder = dto.CardHolder,
                    CardType = dto.CardType,
                    CardStatus = dto.CardStatus
                }
            },
            TotalCount = 1
        };
    }

    public async Task<CardResponse> GetCardByRefAsync(string cardRef, string requestId, string aspspCode, string tppCode)
    {
        var headers = HttpHeaderUtil.BuildOhvpsHeaders(requestId, aspspCode, tppCode, jwsSignature: null);
        var dto = await _hhsClient.GetCardByRefAsync(cardRef, headers);
        return new CardResponse
        {
            CardRef = dto.CardRef,
            CardNumber = dto.CardNumber,
            CardHolder = dto.CardHolder,
            CardType = dto.CardType,
            CardStatus = dto.CardStatus
        };
    }

    public async Task<CardResponse> GetCardDetailAsync(string cardRef, string requestId, string aspspCode, string tppCode)
    {
        var headers = HttpHeaderUtil.BuildOhvpsHeaders(requestId, aspspCode, tppCode, jwsSignature: null);
        var dto = await _hhsClient.GetCardDetailAsync(cardRef, headers);
        return new CardResponse
        {
            CardRef = dto.CardRef,
            CardNumber = dto.CardNumber,
            CardHolder = dto.CardHolder,
            CardType = dto.CardType,
            CardStatus = dto.CardStatus
        };
    }

    public async Task<CardTransactionResponse> GetCardTransactionsAsync(string cardRef, string requestId, string aspspCode, string tppCode)
    {
        var headers = HttpHeaderUtil.BuildOhvpsHeaders(requestId, aspspCode, tppCode, jwsSignature: null);
        var dto = await _hhsClient.GetCardTransactionsAsync(cardRef, headers);
        return new CardTransactionResponse
        {
            CardRef = dto.CardRef
        };
    }
}

