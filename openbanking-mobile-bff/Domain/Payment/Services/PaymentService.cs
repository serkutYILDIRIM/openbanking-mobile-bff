using openbanking_mobile_bff.Domain.Payment.Models.Requests;
using openbanking_mobile_bff.Domain.Payment.Models.Responses;
using openbanking_mobile_bff.Infrastructure.HttpClients.Yos;

namespace openbanking_mobile_bff.Domain.Payment.Services;

public sealed class PaymentService : IPaymentService
{
    private readonly IYosMicroserviceClient _yosClient;

    public PaymentService(IYosMicroserviceClient yosClient)
    {
        _yosClient = yosClient;
    }

    public Task<PaymentResponse> CreatePaymentOrderAsync(PaymentRequest request, string requestId, string aspspCode, string tppCode)
    {
        throw new NotImplementedException();
    }

    public Task<PaymentStatusResponse> GetPaymentOrderAsync(string paymentOrderId, string requestId, string aspspCode, string tppCode)
    {
        throw new NotImplementedException();
    }
}

