using System.Text.Json;
using openbanking_mobile_bff.Domain.Consent.Models.Requests;

namespace openbanking_mobile_bff.Tests.Domain.Consent.Models.Requests;

public sealed class ConsentRequestTests
{
    [Fact]
    public void Constructor_Always_InitializesWithNullValues()
    {
        var request = new ConsentRequest();

        Assert.Null(request.ParticipantInfo);
        Assert.Null(request.Gkd);
        Assert.Null(request.Identity);
        Assert.Null(request.AccountInfo);
        Assert.Null(request.PaymentInfo);
    }

    [Fact]
    public void Properties_WithProvidedValues_PreservesAssignedState()
    {
        var participantInfo = new ConsentParticipantInfo { HhsCode = "hhs-001", YosCode = "yos-001" };
        var gkd = new ConsentGkd { AuthMethod = "redirect", RedirectUri = "https://example.com/callback" };
        var identity = new ConsentIdentity
        {
            IdentityType = "TCKN",
            IdentityValue = "12345678901",
            CustomerType = "individual"
        };
        var accountInfo = new ConsentAccountInfo
        {
            PermissionTypes = ["accounts", "balances"],
            AccessValidUntil = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc)
        };
        var paymentInfo = new ConsentPaymentInfo
        {
            Amount = new ConsentAmount { Amount = "100.50", CurrencyCode = "TRY" },
            PaymentInitiation = new ConsentPaymentInitiation
            {
                Identity = identity,
                Sender = new ConsentParty { Title = "Jane Doe", AccountNumber = "TR111" },
                Receiver = new ConsentParty { Title = "John Smith", AccountNumber = "TR222" }
            }
        };

        var request = new ConsentRequest
        {
            ParticipantInfo = participantInfo,
            Gkd = gkd,
            Identity = identity,
            AccountInfo = accountInfo,
            PaymentInfo = paymentInfo
        };

        Assert.Same(participantInfo, request.ParticipantInfo);
        Assert.Same(gkd, request.Gkd);
        Assert.Same(identity, request.Identity);
        Assert.Same(accountInfo, request.AccountInfo);
        Assert.Same(paymentInfo, request.PaymentInfo);
        Assert.Equal(["accounts", "balances"], request.AccountInfo?.PermissionTypes);
        Assert.Equal("TRY", request.PaymentInfo?.Amount?.CurrencyCode);
        Assert.Equal("TR222", request.PaymentInfo?.PaymentInitiation?.Receiver?.AccountNumber);
    }

    [Fact]
    public void Serialize_WithCompleteRequest_UsesExpectedContractFieldNames()
    {
        var request = new ConsentRequest
        {
            ParticipantInfo = new ConsentParticipantInfo { HhsCode = "hhs-001", YosCode = "yos-001" },
            Gkd = new ConsentGkd { AuthMethod = "redirect", RedirectUri = "https://example.com/callback" },
            Identity = new ConsentIdentity
            {
                IdentityType = "TCKN",
                IdentityValue = "12345678901",
                CustomerType = "individual"
            },
            AccountInfo = new ConsentAccountInfo
            {
                PermissionTypes = ["accounts", "balances"],
                AccessValidUntil = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc)
            },
            PaymentInfo = new ConsentPaymentInfo
            {
                Amount = new ConsentAmount { Amount = "100.50", CurrencyCode = "TRY" },
                PaymentInitiation = new ConsentPaymentInitiation
                {
                    Sender = new ConsentParty { Title = "Jane Doe", AccountNumber = "TR111" },
                    Receiver = new ConsentParty { Title = "John Smith", AccountNumber = "TR222" }
                }
            }
        };

        var json = JsonSerializer.Serialize(request);
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.Equal("hhs-001", root.GetProperty("katilimciBlg").GetProperty("hhsKod").GetString());
        Assert.Equal("redirect", root.GetProperty("gkd").GetProperty("yetYntm").GetString());
        Assert.Equal("TCKN", root.GetProperty("kmlk").GetProperty("kmlkTur").GetString());
        Assert.Equal("accounts", root.GetProperty("hspBlg").GetProperty("iznTur")[0].GetString());
        Assert.Equal("100.50", root.GetProperty("odmBlg").GetProperty("islTtr").GetProperty("ttr").GetString());
        Assert.Equal("Jane Doe", root.GetProperty("odmBlg").GetProperty("odmBsltm").GetProperty("gon").GetProperty("unv").GetString());
    }
}
