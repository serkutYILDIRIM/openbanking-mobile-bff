using openbanking_mobile_bff.Domain.Card.Models.Responses;

namespace openbanking_mobile_bff.Domain.Card.Services;

public interface ICardService
{
    Task<CardListResponse> GetCardsAsync(string requestId, string aspspCode, string tppCode);
    Task<CardResponse> GetCardByRefAsync(string cardRef, string requestId, string aspspCode, string tppCode);
    Task<CardResponse> GetCardDetailAsync(string cardRef, string requestId, string aspspCode, string tppCode);
    Task<CardTransactionResponse> GetCardTransactionsAsync(string cardRef, string requestId, string aspspCode, string tppCode);
}

