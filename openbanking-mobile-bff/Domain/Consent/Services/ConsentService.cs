using openbanking_mobile_bff.Domain.Consent.Models.Requests;
using openbanking_mobile_bff.Domain.Consent.Models.Responses;
using openbanking_mobile_bff.Infrastructure.HttpClients.Yos;

namespace openbanking_mobile_bff.Domain.Consent.Services;

public sealed class ConsentService : IConsentService
{
    private readonly IYosMicroserviceClient _yosClient;

    public ConsentService(IYosMicroserviceClient yosClient)
    {
        _yosClient = yosClient;
    }

    public Task<ConsentResponse> CreateAccountConsentAsync(ConsentRequest request, string requestId, string aspspCode, string tppCode)
    {
        throw new NotImplementedException();
    }

    public Task<ConsentResponse> GetAccountConsentAsync(string consentId, string requestId, string aspspCode, string tppCode)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAccountConsentAsync(string consentId, string requestId, string aspspCode, string tppCode)
    {
        throw new NotImplementedException();
    }

    public Task<ConsentResponse> CreatePaymentConsentAsync(ConsentRequest request, string requestId, string aspspCode, string tppCode)
    {
        throw new NotImplementedException();
    }

    public Task<ConsentResponse> GetPaymentConsentAsync(string consentId, string requestId, string aspspCode, string tppCode)
    {
        throw new NotImplementedException();
    }

    public Task DeletePaymentConsentAsync(string consentId, string requestId, string aspspCode, string tppCode)
    {
        throw new NotImplementedException();
    }
}

