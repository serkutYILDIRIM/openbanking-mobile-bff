using System.Text.Json;
using openbanking_mobile_bff.Domain.Payment.Models.Requests;

namespace openbanking_mobile_bff.Tests.Domain.Payment.Models.Requests;

public sealed class PaymentRequestTests
{
    [Fact]
    public void Constructor_Always_InitializesWithNullValues()
    {
        var request = new PaymentRequest();

        Assert.Null(request.ParticipantInfo);
        Assert.Null(request.Gkd);
        Assert.Null(request.PaymentInfo);
    }

    [Fact]
    public void Properties_WithProvidedValues_PreservesAssignedState()
    {
        var participantInfo = new PaymentParticipantInfo { HhsCode = "hhs-001", YosCode = "yos-001" };
        var gkd = new PaymentGkd { AuthMethod = "redirect", RedirectUri = "https://example.com/callback" };
        var paymentInfo = new PaymentInfo
        {
            Amount = new PaymentAmount { Amount = "1000.50", CurrencyCode = "TRY" },
            PaymentInitiation = new PaymentInitiation
            {
                Sender = new PaymentParty { Title = "Jane Doe", AccountNumber = "TR111" },
                Receiver = new PaymentParty { Title = "John Smith", AccountNumber = "TR222" }
            }
        };

        var request = new PaymentRequest
        {
            ParticipantInfo = participantInfo,
            Gkd = gkd,
            PaymentInfo = paymentInfo
        };

        Assert.Same(participantInfo, request.ParticipantInfo);
        Assert.Same(gkd, request.Gkd);
        Assert.Same(paymentInfo, request.PaymentInfo);
        Assert.Equal("TRY", request.PaymentInfo?.Amount?.CurrencyCode);
        Assert.Equal("Jane Doe", request.PaymentInfo?.PaymentInitiation?.Sender?.Title);
        Assert.Equal("TR222", request.PaymentInfo?.PaymentInitiation?.Receiver?.AccountNumber);
    }

    [Fact]
    public void Serialize_WithCompleteRequest_UsesExpectedContractFieldNames()
    {
        var request = new PaymentRequest
        {
            ParticipantInfo = new PaymentParticipantInfo { HhsCode = "hhs-001", YosCode = "yos-001" },
            Gkd = new PaymentGkd { AuthMethod = "redirect", RedirectUri = "https://example.com/callback" },
            PaymentInfo = new PaymentInfo
            {
                Amount = new PaymentAmount { Amount = "1000.50", CurrencyCode = "TRY" },
                PaymentInitiation = new PaymentInitiation
                {
                    Sender = new PaymentParty { Title = "Jane Doe", AccountNumber = "TR111" },
                    Receiver = new PaymentParty { Title = "John Smith", AccountNumber = "TR222" }
                }
            }
        };

        var json = JsonSerializer.Serialize(request);
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.True(root.TryGetProperty("katilimciBlg", out var participantInfo));
        Assert.True(participantInfo.TryGetProperty("hhsKod", out var hhsCode));
        Assert.Equal("hhs-001", hhsCode.GetString());

        Assert.True(root.TryGetProperty("gkd", out var gkd));
        Assert.True(gkd.TryGetProperty("yetYntm", out var authMethod));
        Assert.Equal("redirect", authMethod.GetString());

        Assert.True(root.TryGetProperty("odmBlg", out var paymentInfo));
        Assert.True(paymentInfo.TryGetProperty("islTtr", out var amount));
        Assert.True(amount.TryGetProperty("prBrm", out var currencyCode));
        Assert.Equal("TRY", currencyCode.GetString());
    }
}

