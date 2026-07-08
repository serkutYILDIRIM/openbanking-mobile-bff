using openbanking_mobile_bff.Common.Constants;
using openbanking_mobile_bff.Domain.Card.Services;
using openbanking_mobile_bff.Infrastructure.HttpClients.Hhs;
using openbanking_mobile_bff.Infrastructure.HttpClients.Hhs.Dtos;

namespace openbanking_mobile_bff.Tests.Domain.Card.Services;

public sealed class CardServiceTests
{
    [Fact]
    public async Task GetCardsAsync_WithCardFromClient_MapsDtoIntoSingleCardResponse()
    {
        var dto = new HhsCardDto
        {
            CardRef = "card-ref-1",
            CardNumber = "4111111111111111",
            CardHolder = "Jane Doe",
            CardType = "credit",
            CardStatus = "active"
        };
        var client = new FakeHhsMicroserviceClient { CardResult = dto };
        var service = new CardService(client);

        var result = await service.GetCardsAsync("req-123", "aspsp-001", "tpp-001");

        Assert.Equal(1, result.TotalCount);
        var card = Assert.Single(result.Cards);
        Assert.Equal("card-ref-1", card.CardRef);
        Assert.Equal("4111111111111111", card.CardNumber);
        Assert.Equal("Jane Doe", card.CardHolder);
        Assert.Equal("credit", card.CardType);
        Assert.Equal("active", card.CardStatus);
    }

    [Fact]
    public async Task GetCardsAsync_BuildsOhvpsHeadersWithoutJwsSignature()
    {
        var client = new FakeHhsMicroserviceClient { CardResult = new HhsCardDto() };
        var service = new CardService(client);

        await service.GetCardsAsync("req-123", "aspsp-001", "tpp-001");

        Assert.NotNull(client.CapturedHeaders);
        Assert.Equal(3, client.CapturedHeaders!.Count);
        Assert.Equal("req-123", client.CapturedHeaders[OhvpsConstants.RequestIdHeader]);
        Assert.Equal("aspsp-001", client.CapturedHeaders[OhvpsConstants.AspspCodeHeader]);
        Assert.Equal("tpp-001", client.CapturedHeaders[OhvpsConstants.TppCodeHeader]);
        Assert.False(client.CapturedHeaders.ContainsKey(OhvpsConstants.JwsSignatureHeader));
    }

    [Fact]
    public async Task GetCardByRefAsync_PassesCardRefToClientAndMapsResponse()
    {
        var dto = new HhsCardDto
        {
            CardRef = "card-ref-9",
            CardNumber = "5500000000000004",
            CardHolder = "John Smith",
            CardType = "debit",
            CardStatus = "passive"
        };
        var client = new FakeHhsMicroserviceClient { CardResult = dto };
        var service = new CardService(client);

        var result = await service.GetCardByRefAsync("card-ref-9", "req-123", "aspsp-001", "tpp-001");

        Assert.Equal("card-ref-9", client.CapturedCardRef);
        Assert.Equal("card-ref-9", result.CardRef);
        Assert.Equal("5500000000000004", result.CardNumber);
        Assert.Equal("John Smith", result.CardHolder);
        Assert.Equal("debit", result.CardType);
        Assert.Equal("passive", result.CardStatus);
    }

    [Fact]
    public async Task GetCardDetailAsync_PassesCardRefToClientAndMapsResponse()
    {
        var dto = new HhsCardDto
        {
            CardRef = "card-ref-7",
            CardNumber = "340000000000009",
            CardHolder = "Alice Brown",
            CardType = "prepaid",
            CardStatus = "active"
        };
        var client = new FakeHhsMicroserviceClient { CardResult = dto };
        var service = new CardService(client);

        var result = await service.GetCardDetailAsync("card-ref-7", "req-123", "aspsp-001", "tpp-001");

        Assert.Equal("card-ref-7", client.CapturedCardRef);
        Assert.Equal("card-ref-7", result.CardRef);
        Assert.Equal("340000000000009", result.CardNumber);
        Assert.Equal("Alice Brown", result.CardHolder);
        Assert.Equal("prepaid", result.CardType);
        Assert.Equal("active", result.CardStatus);
    }

    [Fact]
    public async Task GetCardTransactionsAsync_PassesCardRefToClientAndMapsCardRef()
    {
        var dto = new HhsCardDto { CardRef = "card-ref-3" };
        var client = new FakeHhsMicroserviceClient { CardResult = dto };
        var service = new CardService(client);

        var result = await service.GetCardTransactionsAsync("card-ref-3", "req-123", "aspsp-001", "tpp-001");

        Assert.Equal("card-ref-3", client.CapturedCardRef);
        Assert.Equal("card-ref-3", result.CardRef);
    }

    private sealed class FakeHhsMicroserviceClient : IHhsMicroserviceClient
    {
        public HhsCardDto CardResult { get; set; } = new();

        public Dictionary<string, string>? CapturedHeaders { get; private set; }
        public string? CapturedCardRef { get; private set; }

        public Task<HhsCardDto> GetCardsAsync(Dictionary<string, string> headers)
        {
            CapturedHeaders = headers;
            return Task.FromResult(CardResult);
        }

        public Task<HhsCardDto> GetCardByRefAsync(string cardRef, Dictionary<string, string> headers)
        {
            CapturedCardRef = cardRef;
            CapturedHeaders = headers;
            return Task.FromResult(CardResult);
        }

        public Task<HhsCardDto> GetCardDetailAsync(string cardRef, Dictionary<string, string> headers)
        {
            CapturedCardRef = cardRef;
            CapturedHeaders = headers;
            return Task.FromResult(CardResult);
        }

        public Task<HhsCardDto> GetCardTransactionsAsync(string cardRef, Dictionary<string, string> headers)
        {
            CapturedCardRef = cardRef;
            CapturedHeaders = headers;
            return Task.FromResult(CardResult);
        }

        public Task<HhsAccountDto> GetAccountsAsync(Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<HhsAccountDto> GetAccountByRefAsync(string accountRef, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<HhsBalanceDto> GetBalanceAsync(string accountRef, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<HhsTransactionDto> GetTransactionsAsync(string accountRef, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<HhsPaymentDto> CreatePaymentOrderAsync(HhsPaymentDto request, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<HhsPaymentDto> GetPaymentOrderAsync(string paymentOrderId, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<object?> LinkAccountAsync(object request, Dictionary<string, string> headers) =>
            throw new NotImplementedException();
    }
}
