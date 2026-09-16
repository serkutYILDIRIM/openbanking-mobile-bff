using System.Text.Json;
using openbanking_mobile_bff.Domain.Account.Models.Responses;

namespace openbanking_mobile_bff.Tests.Domain.Account.Models.Responses;

public sealed class AccountResponseTests
{
    [Fact]
    public void Constructor_Always_InitializesWithNullValues()
    {
        var model = new AccountResponse();

        Assert.Null(model.AccountRef);
        Assert.Null(model.AccountNumber);
        Assert.Null(model.AccountOwner);
        Assert.Null(model.AccountType);
        Assert.Null(model.CurrencyCode);
        Assert.Null(model.BranchCode);
        Assert.Null(model.AccountStatus);
    }

    [Fact]
    public void Properties_WithProvidedValues_PreservesAssignedState()
    {
        var model = new AccountResponse
        {
            AccountRef = "acc-ref-123",
            AccountNumber = "1234567890",
            AccountOwner = "Jane Doe",
            AccountType = "current",
            CurrencyCode = "TRY",
            BranchCode = "0001",
            AccountStatus = "active"
        };

        Assert.Equal("acc-ref-123", model.AccountRef);
        Assert.Equal("1234567890", model.AccountNumber);
        Assert.Equal("Jane Doe", model.AccountOwner);
        Assert.Equal("current", model.AccountType);
        Assert.Equal("TRY", model.CurrencyCode);
        Assert.Equal("0001", model.BranchCode);
        Assert.Equal("active", model.AccountStatus);
    }

    [Fact]
    public void Serialize_WithCompleteModel_UsesExpectedContractFieldNames()
    {
        var model = new AccountResponse
        {
            AccountRef = "acc-ref-123",
            AccountNumber = "1234567890",
            AccountOwner = "Jane Doe",
            AccountType = "current",
            CurrencyCode = "TRY",
            BranchCode = "0001",
            AccountStatus = "active"
        };

        var json = JsonSerializer.Serialize(model);
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.True(root.TryGetProperty("hspRef", out var accountRef));
        Assert.Equal("acc-ref-123", accountRef.GetString());
        Assert.True(root.TryGetProperty("hspNo", out var accountNumber));
        Assert.Equal("1234567890", accountNumber.GetString());
        Assert.True(root.TryGetProperty("hspSahibi", out var accountOwner));
        Assert.Equal("Jane Doe", accountOwner.GetString());
        Assert.True(root.TryGetProperty("hspTur", out var accountType));
        Assert.Equal("current", accountType.GetString());
        Assert.True(root.TryGetProperty("prBrm", out var currencyCode));
        Assert.Equal("TRY", currencyCode.GetString());
        Assert.True(root.TryGetProperty("subeKod", out var branchCode));
        Assert.Equal("0001", branchCode.GetString());
        Assert.True(root.TryGetProperty("hspDurum", out var accountStatus));
        Assert.Equal("active", accountStatus.GetString());
    }

    [Fact]
    public void Deserialize_WithValidJson_ReconstructsModelCorrectly()
    {
        var json = """
        {
            "hspRef": "acc-ref-xyz",
            "hspNo": "0987654321",
            "hspSahibi": "John Smith",
            "hspTur": "savings",
            "prBrm": "USD",
            "subeKod": "0002",
            "hspDurum": "passive"
        }
        """;

        var model = JsonSerializer.Deserialize<AccountResponse>(json);

        Assert.NotNull(model);
        Assert.Equal("acc-ref-xyz", model!.AccountRef);
        Assert.Equal("0987654321", model.AccountNumber);
        Assert.Equal("John Smith", model.AccountOwner);
        Assert.Equal("savings", model.AccountType);
        Assert.Equal("USD", model.CurrencyCode);
        Assert.Equal("0002", model.BranchCode);
        Assert.Equal("passive", model.AccountStatus);
    }
}
