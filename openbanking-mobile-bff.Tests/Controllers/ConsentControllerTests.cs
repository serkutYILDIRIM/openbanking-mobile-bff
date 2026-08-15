using Microsoft.AspNetCore.Mvc;
using openbanking_mobile_bff.Controllers;
using openbanking_mobile_bff.Domain.Consent.Models.Requests;
using openbanking_mobile_bff.Domain.Consent.Models.Responses;
using openbanking_mobile_bff.Domain.Consent.Services;
namespace openbanking_mobile_bff.Tests.Controllers;
public sealed class ConsentControllerTests
{
    [Fact]
    public async Task CreateAccountConsent_ReturnsOkWithResponseAndPassesArgumentsToService()
    {
        var request = new ConsentRequest();
        var expected = new ConsentResponse { ConsentInfo = new ConsentInfo { ConsentId = "consent-1" } };
        var service = new FakeConsentService { CreateAccountConsentResult = expected };
        var controller = new ConsentController(service);
        var actionResult = await controller.CreateAccountConsent(request, "req-123", "aspsp-001", "tpp-001");
        var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
        var value = Assert.IsType<ConsentResponse>(ok.Value);
        
        Assert.Same(expected, value);
        Assert.Equal((request, "req-123", "aspsp-001", "tpp-001"), service.CreateAccountConsentArgs);
    }
    [Fact]
    public async Task GetAccountConsent_ReturnsOkWithResponseAndPassesArgumentsToService()
    {
        var expected = new ConsentResponse { ConsentInfo = new ConsentInfo { ConsentId = "consent-2" } };
        var service = new FakeConsentService { GetAccountConsentResult = expected };
        var controller = new ConsentController(service);
        var actionResult = await controller.GetAccountConsent("consent-2", "req-123", "aspsp-001", "tpp-001");
        var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
        var value = Assert.IsType<ConsentResponse>(ok.Value);
        
        Assert.Same(expected, value);
        Assert.Equal(("consent-2", "req-123", "aspsp-001", "tpp-001"), service.GetAccountConsentArgs);
    }
    [Fact]
    public async Task DeleteAccountConsent_ReturnsNoContentAndPassesArgumentsToService()
    {
        var service = new FakeConsentService();
        var controller = new ConsentController(service);
        var result = await controller.DeleteAccountConsent("consent-3", "req-123", "aspsp-001", "tpp-001");
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(("consent-3", "req-123", "aspsp-001", "tpp-001"), service.DeleteAccountConsentArgs);
    }
    [Fact]
    public async Task CreatePaymentConsent_ReturnsOkWithResponseAndPassesArgumentsToService()
    {
        var request = new ConsentRequest();
        var expected = new ConsentResponse { ConsentInfo = new ConsentInfo { ConsentId = "consent-4" } };
        var service = new FakeConsentService { CreatePaymentConsentResult = expected };
        var controller = new ConsentController(service);
        var actionResult = await controller.CreatePaymentConsent(request, "req-123", "aspsp-001", "tpp-001");
        var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
        var value = Assert.IsType<ConsentResponse>(ok.Value);
        Assert.Same(expected, value);
        Assert.Equal((request, "req-123", "aspsp-001", "tpp-001"), service.CreatePaymentConsentArgs);
    }
    [Fact]
    public async Task GetPaymentConsent_ReturnsOkWithResponseAndPassesArgumentsToService()
    {
        var expected = new ConsentResponse { ConsentInfo = new ConsentInfo { ConsentId = "consent-5" } };
        var service = new FakeConsentService { GetPaymentConsentResult = expected };
        var controller = new ConsentController(service);
        var actionResult = await controller.GetPaymentConsent("consent-5", "req-123", "aspsp-001", "tpp-001");
        var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
        var value = Assert.IsType<ConsentResponse>(ok.Value);
        Assert.Same(expected, value);
        Assert.Equal(("consent-5", "req-123", "aspsp-001", "tpp-001"), service.GetPaymentConsentArgs);
    }
    [Fact]
    public async Task DeletePaymentConsent_ReturnsNoContentAndPassesArgumentsToService()
    {
        var service = new FakeConsentService();
        var controller = new ConsentController(service);
        var result = await controller.DeletePaymentConsent("consent-6", "req-123", "aspsp-001", "tpp-001");
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(("consent-6", "req-123", "aspsp-001", "tpp-001"), service.DeletePaymentConsentArgs);
    }
    private sealed class FakeConsentService : IConsentService
    {
        public ConsentResponse CreateAccountConsentResult { get; set; } = new();
        public ConsentResponse GetAccountConsentResult { get; set; } = new();
        public ConsentResponse CreatePaymentConsentResult { get; set; } = new();
        public ConsentResponse GetPaymentConsentResult { get; set; } = new();
        public (ConsentRequest Request, string RequestId, string AspspCode, string TppCode)? CreateAccountConsentArgs { get; private set; }
        public (string ConsentId, string RequestId, string AspspCode, string TppCode)? GetAccountConsentArgs { get; private set; }
        public (string ConsentId, string RequestId, string AspspCode, string TppCode)? DeleteAccountConsentArgs { get; private set; }
        public (ConsentRequest Request, string RequestId, string AspspCode, string TppCode)? CreatePaymentConsentArgs { get; private set; }
        public (string ConsentId, string RequestId, string AspspCode, string TppCode)? GetPaymentConsentArgs { get; private set; }
        public (string ConsentId, string RequestId, string AspspCode, string TppCode)? DeletePaymentConsentArgs { get; private set; }
        public Task<ConsentResponse> CreateAccountConsentAsync(ConsentRequest request, string requestId, string aspspCode, string tppCode)
        {
            CreateAccountConsentArgs = (request, requestId, aspspCode, tppCode);
            return Task.FromResult(CreateAccountConsentResult);
        }
        public Task<ConsentResponse> GetAccountConsentAsync(string consentId, string requestId, string aspspCode, string tppCode)
        {
            GetAccountConsentArgs = (consentId, requestId, aspspCode, tppCode);
            return Task.FromResult(GetAccountConsentResult);
        }
        public Task DeleteAccountConsentAsync(string consentId, string requestId, string aspspCode, string tppCode)
        {
            DeleteAccountConsentArgs = (consentId, requestId, aspspCode, tppCode);
            return Task.CompletedTask;
        }
        public Task<ConsentResponse> CreatePaymentConsentAsync(ConsentRequest request, string requestId, string aspspCode, string tppCode)
        {
            CreatePaymentConsentArgs = (request, requestId, aspspCode, tppCode);
            return Task.FromResult(CreatePaymentConsentResult);
        }
        public Task<ConsentResponse> GetPaymentConsentAsync(string consentId, string requestId, string aspspCode, string tppCode)
        {
            GetPaymentConsentArgs = (consentId, requestId, aspspCode, tppCode);
            return Task.FromResult(GetPaymentConsentResult);
        }
        public Task DeletePaymentConsentAsync(string consentId, string requestId, string aspspCode, string tppCode)
        {
            DeletePaymentConsentArgs = (consentId, requestId, aspspCode, tppCode);
            return Task.CompletedTask;
        }
    }
}
