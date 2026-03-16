namespace openbanking_mobile_bff.Configuration;

public sealed class HhsApiPathOptions
{
    public string AccountsPath { get; set; } = string.Empty;
    public string AccountByRefPath { get; set; } = string.Empty;
    public string BalancePath { get; set; } = string.Empty;
    public string TransactionsPath { get; set; } = string.Empty;
    public string CardsPath { get; set; } = string.Empty;
    public string CardByRefPath { get; set; } = string.Empty;
    public string CardDetailPath { get; set; } = string.Empty;
    public string CardTransactionsPath { get; set; } = string.Empty;
    public string PaymentsPath { get; set; } = string.Empty;
    public string PaymentByIdPath { get; set; } = string.Empty;
    public string AccountLinkPath { get; set; } = string.Empty;
}

