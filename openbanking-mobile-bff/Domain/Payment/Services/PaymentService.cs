using Microsoft.Extensions.Options;
using openbanking_mobile_bff.Common.Utilities;
using openbanking_mobile_bff.Configuration;
using openbanking_mobile_bff.Domain.Payment.Models.Requests;
using openbanking_mobile_bff.Domain.Payment.Models.Responses;
using openbanking_mobile_bff.Infrastructure.HttpClients.Hhs;
using openbanking_mobile_bff.Infrastructure.HttpClients.Hhs.Dtos;
using openbanking_mobile_bff.Infrastructure.HttpClients.Yos;
using openbanking_mobile_bff.Infrastructure.HttpClients.Yos.Dtos;

namespace openbanking_mobile_bff.Domain.Payment.Services;

public sealed class PaymentService : IPaymentService
{
    private readonly IYosMicroserviceClient _yosClient;
    private readonly IHhsMicroserviceClient _hhsClient;
    private readonly BffRoleOptions _roleOptions;

    public PaymentService(
        IYosMicroserviceClient yosClient,
        IHhsMicroserviceClient hhsClient,
        IOptions<BffRoleOptions> roleOptions)
    {
        _yosClient = yosClient;
        _hhsClient = hhsClient;
        _roleOptions = roleOptions.Value;
    }

    public async Task<PaymentResponse> CreatePaymentOrderAsync(PaymentRequest request, string requestId, string aspspCode, string tppCode)
    {
        var headers = HttpHeaderUtil.BuildOhvpsHeaders(requestId, aspspCode, tppCode, jwsSignature: null);

        if (_roleOptions.Role == "HHS")
        {
            var hhsRequest = new HhsPaymentDto
            {
                Amount = request.PaymentInfo?.Amount?.Amount,
                CurrencyCode = request.PaymentInfo?.Amount?.CurrencyCode,
                SenderName = request.PaymentInfo?.PaymentInitiation?.Sender?.Title,
                SenderAccountNumber = request.PaymentInfo?.PaymentInitiation?.Sender?.AccountNumber,
                ReceiverName = request.PaymentInfo?.PaymentInitiation?.Receiver?.Title,
                ReceiverAccountNumber = request.PaymentInfo?.PaymentInitiation?.Receiver?.AccountNumber
            };

            var hhsResult = await _hhsClient.CreatePaymentOrderAsync(hhsRequest, headers);
            return MapHhsPaymentToResponse(hhsResult);
        }

        var yosRequest = new YosPaymentDto
        {
            ParticipantInfo = request.ParticipantInfo != null
                ? new KatilimciBlgDto
                {
                    HhsCode = request.ParticipantInfo.HhsCode,
                    YosCode = request.ParticipantInfo.YosCode
                }
                : null,
            PaymentOrder = request.PaymentInfo != null
                ? new OdemeEmriDto
                {
                    Amount = request.PaymentInfo.Amount != null
                        ? new ParaDto
                        {
                            Amount = request.PaymentInfo.Amount.Amount,
                            CurrencyCode = request.PaymentInfo.Amount.CurrencyCode
                        }
                        : null,
                    Sender = request.PaymentInfo.PaymentInitiation?.Sender != null
                        ? new TarafDto
                        {
                            Title = request.PaymentInfo.PaymentInitiation.Sender.Title,
                            AccountNumber = request.PaymentInfo.PaymentInitiation.Sender.AccountNumber
                        }
                        : null,
                    Receiver = request.PaymentInfo.PaymentInitiation?.Receiver != null
                        ? new TarafDto
                        {
                            Title = request.PaymentInfo.PaymentInitiation.Receiver.Title,
                            AccountNumber = request.PaymentInfo.PaymentInitiation.Receiver.AccountNumber
                        }
                        : null
                }
                : null
        };

        var yosResult = await _yosClient.CreatePaymentOrderAsync(yosRequest, headers);
        return MapYosPaymentToResponse(yosResult);
    }

    public async Task<PaymentStatusResponse> GetPaymentOrderAsync(string paymentOrderId, string requestId, string aspspCode, string tppCode)
    {
        var headers = HttpHeaderUtil.BuildOhvpsHeaders(requestId, aspspCode, tppCode, jwsSignature: null);

        if (_roleOptions.Role == "HHS")
        {
            var hhsResult = await _hhsClient.GetPaymentOrderAsync(paymentOrderId, headers);
            return new PaymentStatusResponse
            {
                PaymentOrderId = hhsResult.PaymentOrderId,
                PaymentStatus = hhsResult.PaymentStatus,
                PaymentOrderTime = hhsResult.PaymentDateTime,
                Amount = new PaymentAmountInfo
                {
                    Amount = hhsResult.Amount,
                    CurrencyCode = hhsResult.CurrencyCode
                }
            };
        }

        var yosResult = await _yosClient.GetPaymentOrderAsync(paymentOrderId, headers);
        return new PaymentStatusResponse
        {
            PaymentOrderId = yosResult.PaymentOrder?.PaymentOrderId,
            PaymentStatus = yosResult.PaymentOrder?.PaymentStatus,
            PaymentOrderTime = yosResult.PaymentOrder?.PaymentOrderTime,
            Amount = yosResult.PaymentOrder?.Amount != null
                ? new PaymentAmountInfo
                {
                    Amount = yosResult.PaymentOrder.Amount.Amount,
                    CurrencyCode = yosResult.PaymentOrder.Amount.CurrencyCode
                }
                : null
        };
    }

    private static PaymentResponse MapHhsPaymentToResponse(HhsPaymentDto dto)
    {
        return new PaymentResponse
        {
            PaymentOrder = new PaymentOrderInfo
            {
                PaymentOrderId = dto.PaymentOrderId,
                PaymentStatus = dto.PaymentStatus,
                PaymentOrderTime = dto.PaymentDateTime,
                Amount = new PaymentAmountInfo
                {
                    Amount = dto.Amount,
                    CurrencyCode = dto.CurrencyCode
                }
            }
        };
    }

    private static PaymentResponse MapYosPaymentToResponse(YosPaymentDto dto)
    {
        return new PaymentResponse
        {
            ConsentInfo = dto.ConsentInfo != null
                ? new PaymentConsentInfo
                {
                    ConsentId = dto.ConsentInfo.ConsentId,
                    ConsentStatus = dto.ConsentInfo.ConsentStatus
                }
                : null,
            ParticipantInfo = dto.ParticipantInfo != null
                ? new Models.Responses.PaymentParticipantInfo
                {
                    HhsCode = dto.ParticipantInfo.HhsCode,
                    YosCode = dto.ParticipantInfo.YosCode
                }
                : null,
            PaymentOrder = dto.PaymentOrder != null
                ? new PaymentOrderInfo
                {
                    PaymentOrderId = dto.PaymentOrder.PaymentOrderId,
                    PaymentOrderTime = dto.PaymentOrder.PaymentOrderTime,
                    PaymentStatus = dto.PaymentOrder.PaymentStatus,
                    Amount = dto.PaymentOrder.Amount != null
                        ? new PaymentAmountInfo
                        {
                            Amount = dto.PaymentOrder.Amount.Amount,
                            CurrencyCode = dto.PaymentOrder.Amount.CurrencyCode
                        }
                        : null
                }
                : null
        };
    }
}

