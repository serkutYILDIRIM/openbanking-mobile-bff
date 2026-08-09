using Microsoft.AspNetCore.Mvc;
using openbanking_mobile_bff.Controllers;
using openbanking_mobile_bff.Domain.Card.Models.Responses;
using openbanking_mobile_bff.Domain.Card.Services;

namespace openbanking_mobile_bff.Tests.Controllers;

public sealed class CardControllerTests
{
    [Fact]
    public async Task GetCards_ReturnsOkWithResponseAndPassesHeadersToService()
    {
        var expected = new CardListResponse
        {
            TotalCount = 1,
            Cards = new List<CardResponse> { new() { CardRef = "card-1" } }
        };
        var service = new FakeCardService { GetCardsResult = expected };
        var controller = new CardController(service);

        var actionResult = await controller.GetCards("req-123", "aspsp-001", "tpp-001");

        var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
        var value = Assert.IsType<CardListResponse>(ok.Value);
        Assert.Same(expected, value);
        Assert.Equal(("req-123", "aspsp-001", "tpp-001"), service.GetCardsArgs);
    }

    [Fact]
    public async Task GetCardByRef_ReturnsOkWithResponseAndPassesArgumentsToService()
    {
        var expected = new CardResponse { CardRef = "card-9" };
        var service = new FakeCardService { GetCardByRefResult = expected };
        var controller = new CardController(service);

        var actionResult = await controller.GetCardByRef("card-9", "req-123", "aspsp-001", "tpp-001");

        var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
        var value = Assert.IsType<CardResponse>(ok.Value);
        Assert.Same(expected, value);
        Assert.Equal(("card-9", "req-123", "aspsp-001", "tpp-001"), service.GetCardByRefArgs);
    }

    [Fact]
    public async Task GetCardDetail_ReturnsOkWithResponseAndPassesArgumentsToService()
    {
        var expected = new CardResponse { CardRef = "card-7" };
        var service = new FakeCardService { GetCardDetailResult = expected };
        var controller = new CardController(service);

        var actionResult = await controller.GetCardDetail("card-7", "req-123", "aspsp-001", "tpp-001");

        var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
        var value = Assert.IsType<CardResponse>(ok.Value);
        Assert.Same(expected, value);
        Assert.Equal(("card-7", "req-123", "aspsp-001", "tpp-001"), service.GetCardDetailArgs);
    }

    [Fact]
    public async Task GetCardTransactions_ReturnsOkWithResponseAndPassesArgumentsToService()
    {
        var expected = new CardTransactionResponse { CardRef = "card-3" };
        var service = new FakeCardService { GetCardTransactionsResult = expected };
        var controller = new CardController(service);

        var actionResult = await controller.GetCardTransactions("card-3", "req-123", "aspsp-001", "tpp-001");

        var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
        var value = Assert.IsType<CardTransactionResponse>(ok.Value);
        Assert.Same(expected, value);
        Assert.Equal(("card-3", "req-123", "aspsp-001", "tpp-001"), service.GetCardTransactionsArgs);
    }

    private sealed class FakeCardService : ICardService
    {
        public CardListResponse GetCardsResult { get; set; } = new();
        public CardResponse GetCardByRefResult { get; set; } = new();
        public CardResponse GetCardDetailResult { get; set; } = new();
        public CardTransactionResponse GetCardTransactionsResult { get; set; } = new();

        public (string RequestId, string AspspCode, string TppCode)? GetCardsArgs { get; private set; }
        public (string CardRef, string RequestId, string AspspCode, string TppCode)? GetCardByRefArgs { get; private set; }
        public (string CardRef, string RequestId, string AspspCode, string TppCode)? GetCardDetailArgs { get; private set; }
        public (string CardRef, string RequestId, string AspspCode, string TppCode)? GetCardTransactionsArgs { get; private set; }

        public Task<CardListResponse> GetCardsAsync(string requestId, string aspspCode, string tppCode)
        {
            GetCardsArgs = (requestId, aspspCode, tppCode);
            return Task.FromResult(GetCardsResult);
        }

        public Task<CardResponse> GetCardByRefAsync(string cardRef, string requestId, string aspspCode, string tppCode)
        {
            GetCardByRefArgs = (cardRef, requestId, aspspCode, tppCode);
            return Task.FromResult(GetCardByRefResult);
        }

        public Task<CardResponse> GetCardDetailAsync(string cardRef, string requestId, string aspspCode, string tppCode)
        {
            GetCardDetailArgs = (cardRef, requestId, aspspCode, tppCode);
            return Task.FromResult(GetCardDetailResult);
        }

        public Task<CardTransactionResponse> GetCardTransactionsAsync(string cardRef, string requestId, string aspspCode, string tppCode)
        {
            GetCardTransactionsArgs = (cardRef, requestId, aspspCode, tppCode);
            return Task.FromResult(GetCardTransactionsResult);
        }
    }
}

