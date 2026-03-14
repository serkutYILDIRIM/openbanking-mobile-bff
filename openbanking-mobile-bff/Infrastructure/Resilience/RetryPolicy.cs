using System.Net;
using openbanking_mobile_bff.Configuration;
using Polly;
using Polly.Extensions.Http;

namespace openbanking_mobile_bff.Infrastructure.Resilience;

public static class RetryPolicy
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(HttpClientPolicyOptions options)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(
                options.RetryCount,
                retryAttempt => TimeSpan.FromSeconds(
                    Math.Pow(options.RetryBaseDelaySeconds, retryAttempt)));
    }
}

