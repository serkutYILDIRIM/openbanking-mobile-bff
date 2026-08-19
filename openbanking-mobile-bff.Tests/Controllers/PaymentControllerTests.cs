using Microsoft.AspNetCore.Mvc;
using openbanking_mobile_bff.Controllers;
using openbanking_mobile_bff.Domain.Payment.Models.Requests;
using openbanking_mobile_bff.Domain.Payment.Models.Responses;
using openbanking_mobile_bff.Domain.Payment.Services;

namespace openbanking_mobile_bff.Tests.Controllers;

public sealed class PaymentControllerTests
{
    [Fact]
    public async Task CreatePaymentOrder_ReturnsOkWithResponseAndPassesArgumentsToService()
    {
        var request = new PaymentRequest();
        var expected = new PaymentResponse
        {
            PaymentOrder = new PaymentOrderInfo { PaymentOrderId = "payment-1" }
        };

        var service = new FakePaymentService { CreatePaymentOrderResult = expected };
        var controller = new PaymentController(service);
        var actionResult = await controller.CreatePaymentOrder(request, "req-123", "aspsp-001", "tpp-001");

        var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
        var value = Assert.IsType<PaymentResponse>(ok.Value);

        Assert.Same(expected, value);
        Assert.Equal((request, "req-123", "aspsp-001", "tpp-001"), service.CreatePaymentOrderArgs);
    }

    [Fact]
    public async Task GetPaymentOrder_ReturnsOkWithResponseAndPassesArgumentsToService()
    {
        var expected = new PaymentStatusResponse
        {
            PaymentOrderId = "payment-2",
            PaymentStatus = "KABL"
        };

        var service = new FakePaymentService { GetPaymentOrderResult = expected };
        var controller = new PaymentController(service);
        var actionResult = await controller.GetPaymentOrder("payment-2", "req-123", "aspsp-001", "tpp-001");

        var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
        var value = Assert.IsType<PaymentStatusResponse>(ok.Value);

        Assert.Same(expected, value);
        Assert.Equal(("payment-2", "req-123", "aspsp-001", "tpp-001"), service.GetPaymentOrderArgs);
    }

    private sealed class FakePaymentService : IPaymentService
    {
        public PaymentResponse CreatePaymentOrderResult { get; set; } = new();
        public PaymentStatusResponse GetPaymentOrderResult { get; set; } = new();

        public (PaymentRequest Request, string RequestId, string AspspCode, string TppCode)? CreatePaymentOrderArgs { get; private set; }
        public (string PaymentOrderId, string RequestId, string AspspCode, string TppCode)? GetPaymentOrderArgs { get; private set; }

        public Task<PaymentResponse> CreatePaymentOrderAsync(PaymentRequest request, string requestId, string aspspCode, string tppCode)
        {
            CreatePaymentOrderArgs = (request, requestId, aspspCode, tppCode);
            return Task.FromResult(CreatePaymentOrderResult);
        }

        public Task<PaymentStatusResponse> GetPaymentOrderAsync(string paymentOrderId, string requestId, string aspspCode, string tppCode)
        {
            GetPaymentOrderArgs = (paymentOrderId, requestId, aspspCode, tppCode);
            return Task.FromResult(GetPaymentOrderResult);
        }
    }
}

