using System.Text.Json;
using openbanking_mobile_bff.Domain.Payment.Models.Responses;

namespace openbanking_mobile_bff.Tests.Domain.Payment.Models.Responses;

public sealed class PaymentStatusResponseTests
{
    [Fact]
    public void Constructor_Always_InitializesWithNullValues()
    {
        var response = new PaymentStatusResponse();

        Assert.Null(response.PaymentOrderId);
        Assert.Null(response.PaymentOrderTime);
        Assert.Null(response.Amount);
    }

    [Fact]
    public void Properties_WithProvidedValues_PreservesAssignedState()
    {
        var amount = new PaymentAmountInfo { Amount = "2500.75", CurrencyCode = "TRY" };
        var orderTime = DateTime.UtcNow;

        var response = new PaymentStatusResponse
        {
            PaymentOrderId = "order-123",
            PaymentStatus = "COMPLETED",
            PaymentOrderTime = orderTime,
            Amount = amount
        };

        Assert.Equal("order-123", response.PaymentOrderId);
        Assert.Equal("COMPLETED", response.PaymentStatus);
        Assert.Equal(orderTime, response.PaymentOrderTime);
        Assert.Same(amount, response.Amount);
        Assert.Equal("2500.75", response.Amount?.Amount);
        Assert.Equal("TRY", response.Amount?.CurrencyCode);
    }

    [Fact]
    public void Serialize_WithCompleteResponse_UsesExpectedContractFieldNames()
    {
        var response = new PaymentStatusResponse
        {
            PaymentOrderId = "order-456",
            PaymentStatus = "PENDING",
            PaymentOrderTime = new DateTime(2025, 1, 15, 10, 30, 0, DateTimeKind.Utc),
            Amount = new PaymentAmountInfo { Amount = "1500.00", CurrencyCode = "USD" }
        };

        var json = JsonSerializer.Serialize(response);
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.True(root.TryGetProperty("odmEmrNo", out var paymentOrderId));
        Assert.Equal("order-456", paymentOrderId.GetString());

        Assert.True(root.TryGetProperty("odmDrm", out var paymentStatus));
        Assert.Equal("PENDING", paymentStatus.GetString());

        Assert.True(root.TryGetProperty("odmEmrZmn", out var paymentOrderTime));
        Assert.Equal("2025-01-15T10:30:00Z", paymentOrderTime.GetString());

        Assert.True(root.TryGetProperty("islTtr", out var amount));
        Assert.True(amount.TryGetProperty("ttr", out var amountValue));
        Assert.Equal("1500.00", amountValue.GetString());
        Assert.True(amount.TryGetProperty("prBrm", out var currencyCode));
        Assert.Equal("USD", currencyCode.GetString());
    }
}
