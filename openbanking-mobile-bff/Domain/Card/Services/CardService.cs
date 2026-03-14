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

    public Task<CardListResponse> GetCardsAsync(string requestId, string aspspCode, string tppCode)
    {
        throw new NotImplementedException();
    }

    public Task<CardResponse> GetCardByRefAsync(string cardRef, string requestId, string aspspCode, string tppCode)
    {
        throw new NotImplementedException();
    }

    public Task<CardResponse> GetCardDetailAsync(string cardRef, string requestId, string aspspCode, string tppCode)
    {
        throw new NotImplementedException();
    }

    public Task<CardTransactionResponse> GetCardTransactionsAsync(string cardRef, string requestId, string aspspCode, string tppCode)
    {
        throw new NotImplementedException();
    }
}

