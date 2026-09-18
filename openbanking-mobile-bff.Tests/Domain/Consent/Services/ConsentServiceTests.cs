using openbanking_mobile_bff.Domain.Consent.Models.Requests;
using openbanking_mobile_bff.Domain.Consent.Services;
using openbanking_mobile_bff.Infrastructure.HttpClients.Yos;
using openbanking_mobile_bff.Infrastructure.HttpClients.Yos.Dtos;

namespace openbanking_mobile_bff.Tests.Domain.Consent.Services;

public sealed class ConsentServiceTests
{
    [Fact]
    public async Task CreateAccountConsentAsync_WithAnyInput_ThrowsNotImplementedException()
    {
        var service = new ConsentService(new FakeYosMicroserviceClient());

        await Assert.ThrowsAsync<NotImplementedException>(() =>
            service.CreateAccountConsentAsync(new ConsentRequest(), "req", "aspsp", "tpp"));
    }

    [Fact]
    public async Task GetAccountConsentAsync_WithAnyInput_ThrowsNotImplementedException()
    {
        var service = new ConsentService(new FakeYosMicroserviceClient());

        await Assert.ThrowsAsync<NotImplementedException>(() =>
            service.GetAccountConsentAsync("consent-1", "req", "aspsp", "tpp"));
    }

    [Fact]
    public async Task DeleteAccountConsentAsync_WithAnyInput_ThrowsNotImplementedException()
    {
        var service = new ConsentService(new FakeYosMicroserviceClient());

        await Assert.ThrowsAsync<NotImplementedException>(() =>
            service.DeleteAccountConsentAsync("consent-1", "req", "aspsp", "tpp"));
    }

    [Fact]
    public async Task CreatePaymentConsentAsync_WithAnyInput_ThrowsNotImplementedException()
    {
        var service = new ConsentService(new FakeYosMicroserviceClient());

        await Assert.ThrowsAsync<NotImplementedException>(() =>
            service.CreatePaymentConsentAsync(new ConsentRequest(), "req", "aspsp", "tpp"));
    }

    [Fact]
    public async Task GetPaymentConsentAsync_WithAnyInput_ThrowsNotImplementedException()
    {
        var service = new ConsentService(new FakeYosMicroserviceClient());

        await Assert.ThrowsAsync<NotImplementedException>(() =>
            service.GetPaymentConsentAsync("consent-1", "req", "aspsp", "tpp"));
    }

    [Fact]
    public async Task DeletePaymentConsentAsync_WithAnyInput_ThrowsNotImplementedException()
    {
        var service = new ConsentService(new FakeYosMicroserviceClient());

        await Assert.ThrowsAsync<NotImplementedException>(() =>
            service.DeletePaymentConsentAsync("consent-1", "req", "aspsp", "tpp"));
    }

    private sealed class FakeYosMicroserviceClient : IYosMicroserviceClient
    {
        public Task<YosConsentDto> CreateAccountConsentAsync(YosConsentDto request, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<YosConsentDto> GetAccountConsentAsync(string consentId, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task DeleteAccountConsentAsync(string consentId, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<YosConsentDto> CreatePaymentConsentAsync(YosConsentDto request, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<YosConsentDto> GetPaymentConsentAsync(string consentId, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task DeletePaymentConsentAsync(string consentId, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<YosPaymentDto> CreatePaymentOrderAsync(YosPaymentDto request, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<YosPaymentDto> GetPaymentOrderAsync(string paymentOrderId, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<YosGkdDto> GetAuthorizationCodeAsync(Dictionary<string, string> headers, string? queryParams = null) =>
            throw new NotImplementedException();

        public Task<YosGkdDto> CreateAccessTokenAsync(YosGkdDto request, Dictionary<string, string> headers) =>
            throw new NotImplementedException();
    }
}
