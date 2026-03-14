using openbanking_mobile_bff.Domain.Payment.Models.Requests;
using openbanking_mobile_bff.Domain.Payment.Models.Responses;

namespace openbanking_mobile_bff.Domain.Payment.Services;

public interface IPaymentService
{
    Task<PaymentResponse> CreatePaymentOrderAsync(PaymentRequest request, string requestId, string aspspCode, string tppCode);
    Task<PaymentStatusResponse> GetPaymentOrderAsync(string paymentOrderId, string requestId, string aspspCode, string tppCode);
}

