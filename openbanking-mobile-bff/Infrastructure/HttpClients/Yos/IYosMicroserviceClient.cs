using openbanking_mobile_bff.Infrastructure.HttpClients.Yos.Dtos;

namespace openbanking_mobile_bff.Infrastructure.HttpClients.Yos;

public interface IYosMicroserviceClient
{
    Task<YosConsentDto> CreateAccountConsentAsync(YosConsentDto request, Dictionary<string, string> headers);
    Task<YosConsentDto> GetAccountConsentAsync(string consentId, Dictionary<string, string> headers);
    Task DeleteAccountConsentAsync(string consentId, Dictionary<string, string> headers);
    Task<YosConsentDto> CreatePaymentConsentAsync(YosConsentDto request, Dictionary<string, string> headers);
    Task<YosConsentDto> GetPaymentConsentAsync(string consentId, Dictionary<string, string> headers);
    Task DeletePaymentConsentAsync(string consentId, Dictionary<string, string> headers);
    Task<YosPaymentDto> CreatePaymentOrderAsync(YosPaymentDto request, Dictionary<string, string> headers);
    Task<YosPaymentDto> GetPaymentOrderAsync(string paymentOrderId, Dictionary<string, string> headers);
    Task<YosGkdDto> GetAuthorizationCodeAsync(Dictionary<string, string> headers, string? queryParams = null);
    Task<YosGkdDto> CreateAccessTokenAsync(YosGkdDto request, Dictionary<string, string> headers);
}

