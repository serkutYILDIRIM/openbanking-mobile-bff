using Microsoft.Extensions.Options;
using openbanking_mobile_bff.Common.Constants;
using openbanking_mobile_bff.Configuration;
using openbanking_mobile_bff.Domain.Payment.Models.Requests;
using openbanking_mobile_bff.Domain.Payment.Services;
using openbanking_mobile_bff.Infrastructure.HttpClients.Hhs;
using openbanking_mobile_bff.Infrastructure.HttpClients.Hhs.Dtos;
using openbanking_mobile_bff.Infrastructure.HttpClients.Yos;
using openbanking_mobile_bff.Infrastructure.HttpClients.Yos.Dtos;

namespace openbanking_mobile_bff.Tests.Domain.Payment.Services;

public sealed class PaymentServiceTests
{
    [Fact]
    public async Task CreatePaymentOrderAsync_HhsRole_MapsRequestToHhsClientAndResponse()
    {
        var request = new PaymentRequest
        {
            PaymentInfo = new PaymentInfo
            {
                Amount = new PaymentAmount { Amount = "100.50", CurrencyCode = "TRY" },
                PaymentInitiation = new PaymentInitiation
                {
                    Sender = new PaymentParty { Title = "Jane Doe", AccountNumber = "1111" },
                    Receiver = new PaymentParty { Title = "John Smith", AccountNumber = "2222" }
                }
            }
        };
        var hhsClient = new FakeHhsMicroserviceClient
        {
            PaymentResult = new HhsPaymentDto
            {
                PaymentOrderId = "order-1",
                PaymentStatus = "completed",
                Amount = "100.50",
                CurrencyCode = "TRY",
                PaymentDateTime = new DateTime(2024, 3, 1, 12, 0, 0, DateTimeKind.Utc)
            }
        };
        var yosClient = new FakeYosMicroserviceClient();
        var service = new PaymentService(yosClient, hhsClient, CreateRoleOptions("HHS"));

        var result = await service.CreatePaymentOrderAsync(request, "req-123", "aspsp-001", "tpp-001");

        Assert.NotNull(hhsClient.CapturedPaymentRequest);
        Assert.Equal("100.50", hhsClient.CapturedPaymentRequest!.Amount);
        Assert.Equal("TRY", hhsClient.CapturedPaymentRequest.CurrencyCode);
        Assert.Equal("Jane Doe", hhsClient.CapturedPaymentRequest.SenderName);
        Assert.Equal("1111", hhsClient.CapturedPaymentRequest.SenderAccountNumber);
        Assert.Equal("John Smith", hhsClient.CapturedPaymentRequest.ReceiverName);
        Assert.Equal("2222", hhsClient.CapturedPaymentRequest.ReceiverAccountNumber);

        Assert.NotNull(result.PaymentOrder);
        Assert.Equal("order-1", result.PaymentOrder!.PaymentOrderId);
        Assert.Equal("completed", result.PaymentOrder.PaymentStatus);
        Assert.Equal(new DateTime(2024, 3, 1, 12, 0, 0, DateTimeKind.Utc), result.PaymentOrder.PaymentOrderTime);
        Assert.NotNull(result.PaymentOrder.Amount);
        Assert.Equal("100.50", result.PaymentOrder.Amount!.Amount);
        Assert.Equal("TRY", result.PaymentOrder.Amount.CurrencyCode);
    }

    [Fact]
    public async Task CreatePaymentOrderAsync_YosRole_MapsRequestToYosClientAndResponse()
    {
        var request = new PaymentRequest
        {
            ParticipantInfo = new openbanking_mobile_bff.Domain.Payment.Models.Requests.PaymentParticipantInfo
            {
                HhsCode = "hhs-01",
                YosCode = "yos-01"
            },
            PaymentInfo = new PaymentInfo
            {
                Amount = new PaymentAmount { Amount = "250.00", CurrencyCode = "USD" },
                PaymentInitiation = new PaymentInitiation
                {
                    Sender = new PaymentParty { Title = "Alice", AccountNumber = "3333" },
                    Receiver = new PaymentParty { Title = "Bob", AccountNumber = "4444" }
                }
            }
        };
        var yosClient = new FakeYosMicroserviceClient
        {
            PaymentResult = new YosPaymentDto
            {
                ConsentInfo = new RizaBilgiDto { ConsentId = "consent-1", ConsentStatus = "A" },
                ParticipantInfo = new KatilimciBlgDto { HhsCode = "hhs-01", YosCode = "yos-01" },
                PaymentOrder = new OdemeEmriDto
                {
                    PaymentOrderId = "order-9",
                    PaymentStatus = "pending",
                    PaymentOrderTime = new DateTime(2024, 4, 10, 9, 30, 0, DateTimeKind.Utc),
                    Amount = new ParaDto { Amount = "250.00", CurrencyCode = "USD" }
                }
            }
        };
        var hhsClient = new FakeHhsMicroserviceClient();
        var service = new PaymentService(yosClient, hhsClient, CreateRoleOptions("YOS"));

        var result = await service.CreatePaymentOrderAsync(request, "req-123", "aspsp-001", "tpp-001");

        Assert.NotNull(yosClient.CapturedPaymentRequest);
        Assert.Equal("hhs-01", yosClient.CapturedPaymentRequest!.ParticipantInfo?.HhsCode);
        Assert.Equal("yos-01", yosClient.CapturedPaymentRequest.ParticipantInfo?.YosCode);
        Assert.Equal("250.00", yosClient.CapturedPaymentRequest.PaymentOrder?.Amount?.Amount);
        Assert.Equal("USD", yosClient.CapturedPaymentRequest.PaymentOrder?.Amount?.CurrencyCode);
        Assert.Equal("Alice", yosClient.CapturedPaymentRequest.PaymentOrder?.Sender?.Title);
        Assert.Equal("Bob", yosClient.CapturedPaymentRequest.PaymentOrder?.Receiver?.Title);

        Assert.Equal("consent-1", result.ConsentInfo?.ConsentId);
        Assert.Equal("A", result.ConsentInfo?.ConsentStatus);
        Assert.Equal("hhs-01", result.ParticipantInfo?.HhsCode);
        Assert.Equal("yos-01", result.ParticipantInfo?.YosCode);
        Assert.Equal("order-9", result.PaymentOrder?.PaymentOrderId);
        Assert.Equal("pending", result.PaymentOrder?.PaymentStatus);
        Assert.Equal("250.00", result.PaymentOrder?.Amount?.Amount);
        Assert.Equal("USD", result.PaymentOrder?.Amount?.CurrencyCode);
    }

    [Fact]
    public async Task CreatePaymentOrderAsync_BuildsOhvpsHeadersWithoutJwsSignature()
    {
        var yosClient = new FakeYosMicroserviceClient { PaymentResult = new YosPaymentDto() };
        var hhsClient = new FakeHhsMicroserviceClient();
        var service = new PaymentService(yosClient, hhsClient, CreateRoleOptions("YOS"));

        await service.CreatePaymentOrderAsync(new PaymentRequest(), "req-123", "aspsp-001", "tpp-001");

        Assert.NotNull(yosClient.CapturedHeaders);
        Assert.Equal(3, yosClient.CapturedHeaders!.Count);
        Assert.Equal("req-123", yosClient.CapturedHeaders[OhvpsConstants.RequestIdHeader]);
        Assert.Equal("aspsp-001", yosClient.CapturedHeaders[OhvpsConstants.AspspCodeHeader]);
        Assert.Equal("tpp-001", yosClient.CapturedHeaders[OhvpsConstants.TppCodeHeader]);
        Assert.False(yosClient.CapturedHeaders.ContainsKey(OhvpsConstants.JwsSignatureHeader));
    }

    [Fact]
    public async Task GetPaymentOrderAsync_HhsRole_PassesOrderIdAndMapsStatusResponse()
    {
        var hhsClient = new FakeHhsMicroserviceClient
        {
            PaymentResult = new HhsPaymentDto
            {
                PaymentOrderId = "order-5",
                PaymentStatus = "settled",
                PaymentDateTime = new DateTime(2024, 5, 20, 14, 45, 0, DateTimeKind.Utc),
                Amount = "999.99",
                CurrencyCode = "EUR"
            }
        };
        var yosClient = new FakeYosMicroserviceClient();
        var service = new PaymentService(yosClient, hhsClient, CreateRoleOptions("HHS"));

        var result = await service.GetPaymentOrderAsync("order-5", "req-123", "aspsp-001", "tpp-001");

        Assert.Equal("order-5", hhsClient.CapturedPaymentOrderId);
        Assert.Equal("order-5", result.PaymentOrderId);
        Assert.Equal("settled", result.PaymentStatus);
        Assert.Equal(new DateTime(2024, 5, 20, 14, 45, 0, DateTimeKind.Utc), result.PaymentOrderTime);
        Assert.Equal("999.99", result.Amount?.Amount);
        Assert.Equal("EUR", result.Amount?.CurrencyCode);
    }

    [Fact]
    public async Task GetPaymentOrderAsync_YosRole_PassesOrderIdAndMapsStatusResponse()
    {
        var yosClient = new FakeYosMicroserviceClient
        {
            PaymentResult = new YosPaymentDto
            {
                PaymentOrder = new OdemeEmriDto
                {
                    PaymentOrderId = "order-7",
                    PaymentStatus = "processing",
                    PaymentOrderTime = new DateTime(2024, 6, 15, 8, 0, 0, DateTimeKind.Utc),
                    Amount = new ParaDto { Amount = "42.00", CurrencyCode = "TRY" }
                }
            }
        };
        var hhsClient = new FakeHhsMicroserviceClient();
        var service = new PaymentService(yosClient, hhsClient, CreateRoleOptions("YOS"));

        var result = await service.GetPaymentOrderAsync("order-7", "req-123", "aspsp-001", "tpp-001");

        Assert.Equal("order-7", yosClient.CapturedPaymentOrderId);
        Assert.Equal("order-7", result.PaymentOrderId);
        Assert.Equal("processing", result.PaymentStatus);
        Assert.Equal(new DateTime(2024, 6, 15, 8, 0, 0, DateTimeKind.Utc), result.PaymentOrderTime);
        Assert.Equal("42.00", result.Amount?.Amount);
        Assert.Equal("TRY", result.Amount?.CurrencyCode);
    }

    private static IOptions<BffRoleOptions> CreateRoleOptions(string role) =>
        Options.Create(new BffRoleOptions { Role = role });

    private sealed class FakeHhsMicroserviceClient : IHhsMicroserviceClient
    {
        public HhsPaymentDto PaymentResult { get; set; } = new();

        public Dictionary<string, string>? CapturedHeaders { get; private set; }
        public HhsPaymentDto? CapturedPaymentRequest { get; private set; }
        public string? CapturedPaymentOrderId { get; private set; }

        public Task<HhsPaymentDto> CreatePaymentOrderAsync(HhsPaymentDto request, Dictionary<string, string> headers)
        {
            CapturedPaymentRequest = request;
            CapturedHeaders = headers;
            return Task.FromResult(PaymentResult);
        }

        public Task<HhsPaymentDto> GetPaymentOrderAsync(string paymentOrderId, Dictionary<string, string> headers)
        {
            CapturedPaymentOrderId = paymentOrderId;
            CapturedHeaders = headers;
            return Task.FromResult(PaymentResult);
        }

        public Task<HhsAccountDto> GetAccountsAsync(Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<HhsAccountDto> GetAccountByRefAsync(string accountRef, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<HhsBalanceDto> GetBalanceAsync(string accountRef, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<HhsTransactionDto> GetTransactionsAsync(string accountRef, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<HhsCardDto> GetCardsAsync(Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<HhsCardDto> GetCardByRefAsync(string cardRef, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<HhsCardDto> GetCardDetailAsync(string cardRef, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<HhsCardDto> GetCardTransactionsAsync(string cardRef, Dictionary<string, string> headers) =>
            throw new NotImplementedException();

        public Task<object?> LinkAccountAsync(object request, Dictionary<string, string> headers) =>
            throw new NotImplementedException();
    }

    private sealed class FakeYosMicroserviceClient : IYosMicroserviceClient
    {
        public YosPaymentDto PaymentResult { get; set; } = new();

        public Dictionary<string, string>? CapturedHeaders { get; private set; }
        public YosPaymentDto? CapturedPaymentRequest { get; private set; }
        public string? CapturedPaymentOrderId { get; private set; }

        public Task<YosPaymentDto> CreatePaymentOrderAsync(YosPaymentDto request, Dictionary<string, string> headers)
        {
            CapturedPaymentRequest = request;
            CapturedHeaders = headers;
            return Task.FromResult(PaymentResult);
        }

        public Task<YosPaymentDto> GetPaymentOrderAsync(string paymentOrderId, Dictionary<string, string> headers)
        {
            CapturedPaymentOrderId = paymentOrderId;
            CapturedHeaders = headers;
            return Task.FromResult(PaymentResult);
        }

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

        public Task<YosGkdDto> GetAuthorizationCodeAsync(Dictionary<string, string> headers, string? queryParams = null) =>
            throw new NotImplementedException();

        public Task<YosGkdDto> CreateAccessTokenAsync(YosGkdDto request, Dictionary<string, string> headers) =>
            throw new NotImplementedException();
    }
}
