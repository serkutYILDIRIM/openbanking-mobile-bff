using openbanking_mobile_bff.Configuration;
using Polly;
using Polly.Extensions.Http;

namespace openbanking_mobile_bff.Infrastructure.Resilience;

public static class CircuitBreakerPolicy
{
    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(HttpClientPolicyOptions options)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                options.CircuitBreakerThreshold,
                TimeSpan.FromSeconds(options.CircuitBreakerDurationSeconds));
    }
}

