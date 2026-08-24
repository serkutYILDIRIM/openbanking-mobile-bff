using openbanking_mobile_bff.Configuration;

namespace openbanking_mobile_bff.Tests.Configuration;

public sealed class HhsApiPathOptionsTests
{
    [Fact]
    public void Constructor_Always_InitializesDefaultValues()
    {
        var options = new HhsApiPathOptions();

        Assert.Equal(string.Empty, options.AccountsPath);
        Assert.Equal(string.Empty, options.AccountByRefPath);
        Assert.Equal(string.Empty, options.BalancePath);
        Assert.Equal(string.Empty, options.TransactionsPath);
        Assert.Equal(string.Empty, options.CardsPath);
        
        Assert.Equal(string.Empty, options.CardByRefPath);
        Assert.Equal(string.Empty, options.CardDetailPath);
        Assert.Equal(string.Empty, options.CardTransactionsPath);
        Assert.Equal(string.Empty, options.PaymentsPath);
        Assert.Equal(string.Empty, options.PaymentByIdPath);
        Assert.Equal(string.Empty, options.AccountLinkPath);
    }

    [Fact]
    public void Properties_WithProvidedValues_PreservesAssignedState()
    {
        var options = new HhsApiPathOptions
        {
            AccountsPath = "/accounts",
            AccountByRefPath = "/accounts/{accountRef}",
            BalancePath = "/accounts/{accountRef}/balance",
            TransactionsPath = "/accounts/{accountRef}/transactions",
            CardsPath = "/cards",
            
            CardByRefPath = "/cards/{cardRef}",
            CardDetailPath = "/cards/{cardRef}/details",
            CardTransactionsPath = "/cards/{cardRef}/transactions",
            PaymentsPath = "/payments",
            PaymentByIdPath = "/payments/{paymentId}",
            AccountLinkPath = "/consents/account-link"
        };

        Assert.Equal("/accounts", options.AccountsPath);
        Assert.Equal("/accounts/{accountRef}", options.AccountByRefPath);
        Assert.Equal("/accounts/{accountRef}/balance", options.BalancePath);
        Assert.Equal("/accounts/{accountRef}/transactions", options.TransactionsPath);
        Assert.Equal("/cards", options.CardsPath);
        Assert.Equal("/cards/{cardRef}", options.CardByRefPath);
        Assert.Equal("/cards/{cardRef}/details", options.CardDetailPath);
        Assert.Equal("/cards/{cardRef}/transactions", options.CardTransactionsPath);
        Assert.Equal("/payments", options.PaymentsPath);
        Assert.Equal("/payments/{paymentId}", options.PaymentByIdPath);
        Assert.Equal("/consents/account-link", options.AccountLinkPath);
    }

    [Theory]
    [InlineData("", "", "", "", "", "", "", "", "", "", "")]
    [InlineData(" ", "\t", "\r\n", "a", "b", "c", "d", "e", "f", "g", "h")]
    public void Properties_WithEdgeValues_PreservesAssignedState(
        string accountsPath,
        string accountByRefPath,
        string balancePath,
        string transactionsPath,
        string cardsPath,
        string cardByRefPath,
        string cardDetailPath,
        string cardTransactionsPath,
        string paymentsPath,
        string paymentByIdPath,
        string accountLinkPath)
    {
        var options = new HhsApiPathOptions
        {
            AccountsPath = accountsPath,
            AccountByRefPath = accountByRefPath,
            BalancePath = balancePath,
            TransactionsPath = transactionsPath,
            CardsPath = cardsPath,
            CardByRefPath = cardByRefPath,
            CardDetailPath = cardDetailPath,
            CardTransactionsPath = cardTransactionsPath,
            PaymentsPath = paymentsPath,
            PaymentByIdPath = paymentByIdPath,
            AccountLinkPath = accountLinkPath
        };

        Assert.Equal(accountsPath, options.AccountsPath);
        Assert.Equal(accountByRefPath, options.AccountByRefPath);
        Assert.Equal(balancePath, options.BalancePath);
        Assert.Equal(transactionsPath, options.TransactionsPath);
        Assert.Equal(cardsPath, options.CardsPath);
        Assert.Equal(cardByRefPath, options.CardByRefPath);
        
        Assert.Equal(cardDetailPath, options.CardDetailPath);
        Assert.Equal(cardTransactionsPath, options.CardTransactionsPath);
        Assert.Equal(paymentsPath, options.PaymentsPath);
        Assert.Equal(paymentByIdPath, options.PaymentByIdPath);
        Assert.Equal(accountLinkPath, options.AccountLinkPath);
    }
}

