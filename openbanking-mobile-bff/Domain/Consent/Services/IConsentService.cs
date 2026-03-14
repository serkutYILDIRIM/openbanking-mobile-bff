using openbanking_mobile_bff.Domain.Consent.Models.Requests;
using openbanking_mobile_bff.Domain.Consent.Models.Responses;

namespace openbanking_mobile_bff.Domain.Consent.Services;

public interface IConsentService
{
    Task<ConsentResponse> CreateAccountConsentAsync(ConsentRequest request, string requestId, string aspspCode, string tppCode);
    Task<ConsentResponse> GetAccountConsentAsync(string consentId, string requestId, string aspspCode, string tppCode);
    Task DeleteAccountConsentAsync(string consentId, string requestId, string aspspCode, string tppCode);
    Task<ConsentResponse> CreatePaymentConsentAsync(ConsentRequest request, string requestId, string aspspCode, string tppCode);
    Task<ConsentResponse> GetPaymentConsentAsync(string consentId, string requestId, string aspspCode, string tppCode);
    Task DeletePaymentConsentAsync(string consentId, string requestId, string aspspCode, string tppCode);
}

