using System.Text.Json;
using openbanking_mobile_bff.Domain.Card.Models.Responses;

namespace openbanking_mobile_bff.Tests.Domain.Card.Models.Responses;

public sealed class CardListResponseTests
{
    [Fact]
    public void Constructor_Always_InitializesWithEmptyCollectionAndDefaultCount()
    {
        var response = new CardListResponse();

        Assert.NotNull(response.Cards);
        Assert.Empty(response.Cards);
        Assert.Equal(0, response.TotalCount);
    }

    [Fact]
    public void Properties_WithProvidedValues_PreservesAssignedState()
    {
        var response = new CardListResponse
        {
            Cards =
            [
                new CardResponse
                {
                    CardRef = "card-ref-123",
                    CardNumber = "4111111111111111",
                    CardHolder = "Jane Doe",
                    CardType = "credit",
                    CardStatus = "active"
                }
            ],
            TotalCount = 1
        };

        Assert.Single(response.Cards);
        Assert.Equal("card-ref-123", response.Cards[0].CardRef);
        Assert.Equal("4111111111111111", response.Cards[0].CardNumber);
        Assert.Equal("Jane Doe", response.Cards[0].CardHolder);
        Assert.Equal("credit", response.Cards[0].CardType);
        Assert.Equal("active", response.Cards[0].CardStatus);
        Assert.Equal(1, response.TotalCount);
    }

    [Fact]
    public void Serialize_WithCardsAndTotalCount_UsesExpectedContractFieldNames()
    {
        var response = new CardListResponse
        {
            Cards =
            [
                new CardResponse
                {
                    CardRef = "card-ref-123",
                    CardNumber = "4111111111111111",
                    CardHolder = "Jane Doe",
                    CardType = "credit",
                    CardStatus = "active"
                }
            ],
            TotalCount = 1
        };

        var json = JsonSerializer.Serialize(response);
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.True(root.TryGetProperty("kartlar", out var cards));
        Assert.Equal(JsonValueKind.Array, cards.ValueKind);
        Assert.Single(cards.EnumerateArray());

        var firstCard = cards[0];
        Assert.True(firstCard.TryGetProperty("krtRef", out var cardRef));
        Assert.Equal("card-ref-123", cardRef.GetString());
        Assert.True(firstCard.TryGetProperty("krtNo", out var cardNumber));
        Assert.Equal("4111111111111111", cardNumber.GetString());
        Assert.True(firstCard.TryGetProperty("krtSahibi", out var cardHolder));
        Assert.Equal("Jane Doe", cardHolder.GetString());
        Assert.True(firstCard.TryGetProperty("krtTur", out var cardType));
        Assert.Equal("credit", cardType.GetString());
        Assert.True(firstCard.TryGetProperty("krtDurum", out var cardStatus));
        Assert.Equal("active", cardStatus.GetString());
        Assert.True(root.TryGetProperty("toplamKayitSayisi", out var totalCount));
        Assert.Equal(1, totalCount.GetInt32());
    }

    [Fact]
    public void Deserialize_WithValidJson_ReconstructsModelCorrectly()
    {
        const string json = """
                            {
                              "kartlar": [
                                {
                                  "krtRef": "card-ref-xyz",
                                  "krtNo": "5454545454545454",
                                  "krtSahibi": "John Smith",
                                  "krtTur": "debit",
                                  "krtDurum": "passive"
                                }
                              ],
                              "toplamKayitSayisi": 1
                            }
                            """;

        var response = JsonSerializer.Deserialize<CardListResponse>(json);

        Assert.NotNull(response);
        Assert.Single(response!.Cards);
        Assert.Equal("card-ref-xyz", response.Cards[0].CardRef);
        Assert.Equal("5454545454545454", response.Cards[0].CardNumber);
        Assert.Equal("John Smith", response.Cards[0].CardHolder);
        Assert.Equal("debit", response.Cards[0].CardType);
        Assert.Equal("passive", response.Cards[0].CardStatus);
        Assert.Equal(1, response.TotalCount);
    }
}