using System.Text.Json;
using openbanking_mobile_bff.Domain.Payment.Models.Responses;

namespace openbanking_mobile_bff.Tests.Domain.Payment.Models.Responses;

public sealed class PaymentResponseTests
{
    [Fact]
    public void Constructor_Always_InitializesWithNullValues()
    {
        var response = new PaymentResponse();
        Assert.Null(response.ConsentInfo);
        Assert.Null(response.ParticipantInfo);
        Assert.Null(response.PaymentOrder);
    }

    [Fact]
    public void Properties_WithProvidedValues_PreservesAssignedState()
    {
        var consentInfo = new PaymentConsentInfo { ConsentId = "consent-123", ConsentStatus = "approved" };
        var participantInfo = new PaymentParticipantInfo { HhsCode = "hhs-001", YosCode = "yos-001" };
        var paymentOrder = new PaymentOrderInfo
        {
            PaymentOrderId = "order-456",
            PaymentOrderTime = new DateTime(2026, 9, 5, 10, 30, 0),
            PaymentStatus = "completed",
            Amount = new PaymentAmountInfo { Amount = "1000.50", CurrencyCode = "TRY" }
        };

        var response = new PaymentResponse
        {
            ConsentInfo = consentInfo,
            ParticipantInfo = participantInfo,
            PaymentOrder = paymentOrder
        };

        Assert.Same(consentInfo, response.ConsentInfo);
        Assert.Same(participantInfo, response.ParticipantInfo);
        Assert.Same(paymentOrder, response.PaymentOrder);
        Assert.Equal("consent-123", response.ConsentInfo?.ConsentId);
        Assert.Equal("hhs-001", response.ParticipantInfo?.HhsCode);
        Assert.Equal("order-456", response.PaymentOrder?.PaymentOrderId);
        Assert.Equal("1000.50", response.PaymentOrder?.Amount?.Amount);
    }

    [Fact]
    public void Serialize_WithCompleteResponse_UsesExpectedContractFieldNames()
    {
        var response = new PaymentResponse
        {
            ConsentInfo = new PaymentConsentInfo { ConsentId = "consent-123", ConsentStatus = "approved" },
            ParticipantInfo = new PaymentParticipantInfo { HhsCode = "hhs-001", YosCode = "yos-001" },
            PaymentOrder = new PaymentOrderInfo
            {
                PaymentOrderId = "order-456",
                PaymentOrderTime = new DateTime(2026, 9, 5, 10, 30, 0),
                PaymentStatus = "completed",
                Amount = new PaymentAmountInfo { Amount = "1000.50", CurrencyCode = "TRY" }
            }
        };

        var json = JsonSerializer.Serialize(response);
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.True(root.TryGetProperty("rzBlg", out var consentInfo));
        Assert.True(consentInfo.TryGetProperty("rizaNo", out var consentId));
        Assert.Equal("consent-123", consentId.GetString());
        Assert.True(root.TryGetProperty("katilimciBlg", out var participantInfo));
        Assert.True(participantInfo.TryGetProperty("hhsKod", out var hhsCode));
        Assert.Equal("hhs-001", hhsCode.GetString());
        Assert.True(root.TryGetProperty("odmEmr", out var paymentOrder));
        Assert.True(paymentOrder.TryGetProperty("odmEmrNo", out var paymentOrderId));
        Assert.Equal("order-456", paymentOrderId.GetString());
        Assert.True(paymentOrder.TryGetProperty("odmDrm", out var paymentStatus));
        Assert.Equal("completed", paymentStatus.GetString());
    }

    [Fact]
    public void PaymentConsentInfo_Constructor_InitializesWithNullValues()
    {
        var consentInfo = new PaymentConsentInfo();

        Assert.Null(consentInfo.ConsentId);
        Assert.Null(consentInfo.ConsentStatus);
    }

    [Fact]
    public void PaymentConsentInfo_Properties_WithProvidedValues_PreservesAssignedState()
    {
        var consentInfo = new PaymentConsentInfo
        {
            ConsentId = "consent-789",
            ConsentStatus = "pending"
        };

        Assert.Equal("consent-789", consentInfo.ConsentId);
        Assert.Equal("pending", consentInfo.ConsentStatus);
    }

    [Fact]
    public void PaymentParticipantInfo_Constructor_InitializesWithNullValues()
    {
        var participantInfo = new PaymentParticipantInfo();
        Assert.Null(participantInfo.HhsCode);
        Assert.Null(participantInfo.YosCode);
    }

    [Fact]
    public void PaymentParticipantInfo_Properties_WithProvidedValues_PreservesAssignedState()
    {
        var participantInfo = new PaymentParticipantInfo
        {
            HhsCode = "hhs-999",
            YosCode = "yos-888"
        };

        Assert.Equal("hhs-999", participantInfo.HhsCode);
        Assert.Equal("yos-888", participantInfo.YosCode);
    }

    [Fact]
    public void PaymentOrderInfo_Constructor_InitializesWithNullValues()
    {
        var paymentOrder = new PaymentOrderInfo();

        Assert.Null(paymentOrder.PaymentOrderId);
        Assert.Null(paymentOrder.PaymentOrderTime);
        Assert.Null(paymentOrder.PaymentStatus);
        Assert.Null(paymentOrder.Amount);
    }

    [Fact]
    public void PaymentOrderInfo_Properties_WithProvidedValues_PreservesAssignedState()
    {
        var amount = new PaymentAmountInfo { Amount = "5000.00", CurrencyCode = "EUR" };
        var paymentOrderTime = new DateTime(2026, 9, 5, 15, 45, 30);

        var paymentOrder = new PaymentOrderInfo
        {
            PaymentOrderId = "order-xyz",
            PaymentOrderTime = paymentOrderTime,
            PaymentStatus = "failed",
            Amount = amount
        };

        Assert.Equal("order-xyz", paymentOrder.PaymentOrderId);
        Assert.Equal(paymentOrderTime, paymentOrder.PaymentOrderTime);
        Assert.Equal("failed", paymentOrder.PaymentStatus);
        Assert.Same(amount, paymentOrder.Amount);
    }

    [Fact]
    public void PaymentAmountInfo_Constructor_InitializesWithNullValues()
    {
        var amount = new PaymentAmountInfo();

        Assert.Null(amount.Amount);
        Assert.Null(amount.CurrencyCode);
    }

    [Theory]
    [InlineData("100.00", "TRY")]
    [InlineData("0.01", "USD")]
    [InlineData("999999.99", "EUR")]
    public void PaymentAmountInfo_Properties_WithVariousValues_PreservesAssignedState(string amountValue, string currencyCode)
    {
        var amount = new PaymentAmountInfo
        {
            Amount = amountValue,
            CurrencyCode = currencyCode
        };

        Assert.Equal(amountValue, amount.Amount);
        Assert.Equal(currencyCode, amount.CurrencyCode);
    }

    [Fact]
    public void Serialize_WithNullNestedObjects_ProduceValidJson()
    {
        var response = new PaymentResponse
        {
            ConsentInfo = null,
            ParticipantInfo = null,
            PaymentOrder = null
        };

        var json = JsonSerializer.Serialize(response);
        Assert.NotEmpty(json);

        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        Assert.Equal(3, root.EnumerateObject().Count());
    }

    [Fact]
    public void Deserialize_WithValidJson_ReconstructsResponseCorrectly()
    {
        var json = """
        {
            "rzBlg": {
                "rizaNo": "consent-test",
                "rizaDrm": "approved"
            },
            "katilimciBlg": {
                "hhsKod": "hhs-test",
                "yosKod": "yos-test"
            },
            "odmEmr": {
                "odmEmrNo": "order-test",
                "odmDrm": "completed",
                "odmEmrZmn": "2026-09-05T12:00:00",
                "islTtr": {
                    "ttr": "2000.00",
                    "prBrm": "TRY"
                }
            }
        }
        """;

        var response = JsonSerializer.Deserialize<PaymentResponse>(json);

        Assert.NotNull(response);
        Assert.NotNull(response.ConsentInfo);
        Assert.Equal("consent-test", response.ConsentInfo?.ConsentId);
        Assert.Equal("approved", response.ConsentInfo?.ConsentStatus);
        
        Assert.NotNull(response.ParticipantInfo);
        Assert.Equal("hhs-test", response.ParticipantInfo?.HhsCode);
        Assert.Equal("yos-test", response.ParticipantInfo?.YosCode);
        
        Assert.NotNull(response.PaymentOrder);
        Assert.Equal("order-test", response.PaymentOrder?.PaymentOrderId);
        Assert.Equal("completed", response.PaymentOrder?.PaymentStatus);
        Assert.NotNull(response.PaymentOrder?.Amount);
        Assert.Equal("2000.00", response.PaymentOrder?.Amount?.Amount);
        Assert.Equal("TRY", response.PaymentOrder?.Amount?.CurrencyCode);
    }
}


