using System.Text.Json;
using openbanking_mobile_bff.Domain.Card.Models.Responses;

namespace openbanking_mobile_bff.Tests.Domain.Card.Models.Responses;

public sealed class CardResponseTests
{
    [Fact]
    public void Constructor_Always_InitializesWithNullValues()
    {
        var response = new CardResponse();

        Assert.Null(response.CardRef);
        Assert.Null(response.CardNumber);
        Assert.Null(response.CardHolder);
        Assert.Null(response.CardType);
        Assert.Null(response.CardStatus);
    }

    [Fact]
    public void Properties_WithProvidedValues_PreservesAssignedState()
    {
        var response = new CardResponse
        {
            CardRef = "card-ref-123",
            CardNumber = "4111111111111111",
            CardHolder = "Jane Doe",
            CardType = "credit",
            CardStatus = "active"
        };

        Assert.Equal("card-ref-123", response.CardRef);
        Assert.Equal("4111111111111111", response.CardNumber);
        Assert.Equal("Jane Doe", response.CardHolder);
        Assert.Equal("credit", response.CardType);
        Assert.Equal("active", response.CardStatus);
    }

    [Fact]
    public void Serialize_WithCardProperties_UsesExpectedContractFieldNames()
    {
        var response = new CardResponse
        {
            CardRef = "card-ref-123",
            CardNumber = "4111111111111111",
            CardHolder = "Jane Doe",
            CardType = "credit",
            CardStatus = "active"
        };

        var json = JsonSerializer.Serialize(response);
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.True(root.TryGetProperty("krtRef", out var cardRef));
        Assert.Equal("card-ref-123", cardRef.GetString());
        Assert.True(root.TryGetProperty("krtNo", out var cardNumber));
        Assert.Equal("4111111111111111", cardNumber.GetString());
        Assert.True(root.TryGetProperty("krtSahibi", out var cardHolder));
        Assert.Equal("Jane Doe", cardHolder.GetString());
        Assert.True(root.TryGetProperty("krtTur", out var cardType));
        Assert.Equal("credit", cardType.GetString());
        Assert.True(root.TryGetProperty("krtDurum", out var cardStatus));
        Assert.Equal("active", cardStatus.GetString());
    }

    [Fact]
    public void Deserialize_WithExpectedContractFieldNames_MapsIntoProperties()
    {
        const string json = """
                            {
                              "krtRef": "card-ref-123",
                              "krtNo": "4111111111111111",
                              "krtSahibi": "Jane Doe",
                              "krtTur": "credit",
                              "krtDurum": "active"
                            }
                            """;

        var response = JsonSerializer.Deserialize<CardResponse>(json);

        Assert.NotNull(response);
        Assert.Equal("card-ref-123", response.CardRef);
        Assert.Equal("4111111111111111", response.CardNumber);
        Assert.Equal("Jane Doe", response.CardHolder);
        Assert.Equal("credit", response.CardType);
        Assert.Equal("active", response.CardStatus);
    }
}


