using openbanking_mobile_bff.Configuration;
using openbanking_mobile_bff.Infrastructure.HttpClients;
using openbanking_mobile_bff.Infrastructure.Resilience;

namespace openbanking_mobile_bff.Extensions;

public static class HttpClientExtensions
{
    public static IServiceCollection AddBffHttpClients(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var endpointOptions = configuration
            .GetSection("MicroserviceEndpoints")
            .Get<MicroserviceEndpointOptions>() ?? new MicroserviceEndpointOptions();

        var policyOptions = configuration
            .GetSection("HttpClientPolicy")
            .Get<HttpClientPolicyOptions>() ?? new HttpClientPolicyOptions
            {
                RetryCount = 3,
                RetryBaseDelaySeconds = 2,
                CircuitBreakerThreshold = 5,
                CircuitBreakerDurationSeconds = 30,
                TimeoutSeconds = 30
            };

        services
            .AddHttpClient<IYosMicroserviceClient, YosMicroserviceClient>(client =>
            {
                client.BaseAddress = new Uri(endpointOptions.YosMicroserviceBaseUrl);
            })
            .AddPolicyHandler(RetryPolicy.GetRetryPolicy(policyOptions))
            .AddPolicyHandler(CircuitBreakerPolicy.GetCircuitBreakerPolicy(policyOptions))
            .AddPolicyHandler(TimeoutPolicy.GetTimeoutPolicy(policyOptions));

        services
            .AddHttpClient<IHhsMicroserviceClient, HhsMicroserviceClient>(client =>
            {
                client.BaseAddress = new Uri(endpointOptions.HhsMicroserviceBaseUrl);
            })
            .AddPolicyHandler(RetryPolicy.GetRetryPolicy(policyOptions))
            .AddPolicyHandler(CircuitBreakerPolicy.GetCircuitBreakerPolicy(policyOptions))
            .AddPolicyHandler(TimeoutPolicy.GetTimeoutPolicy(policyOptions));

        services
            .AddHttpClient<IApiGatewayClient, ApiGatewayClient>(client =>
            {
                client.BaseAddress = new Uri(endpointOptions.ApiGatewayBaseUrl);
            })
            .AddPolicyHandler(RetryPolicy.GetRetryPolicy(policyOptions))
            .AddPolicyHandler(CircuitBreakerPolicy.GetCircuitBreakerPolicy(policyOptions))
            .AddPolicyHandler(TimeoutPolicy.GetTimeoutPolicy(policyOptions));

        return services;
    }
}

