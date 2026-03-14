using openbanking_mobile_bff.Configuration;
using Polly;
using Polly.Timeout;

namespace openbanking_mobile_bff.Infrastructure.Resilience;

public static class TimeoutPolicy
{
    public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(HttpClientPolicyOptions options)
    {
        return Policy.TimeoutAsync<HttpResponseMessage>(
            TimeSpan.FromSeconds(options.TimeoutSeconds),
            TimeoutStrategy.Optimistic);
    }
}

