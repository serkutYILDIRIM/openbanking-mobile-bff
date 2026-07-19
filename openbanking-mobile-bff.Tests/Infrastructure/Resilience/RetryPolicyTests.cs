using System.Net;
using openbanking_mobile_bff.Configuration;
using openbanking_mobile_bff.Infrastructure.Resilience;

namespace openbanking_mobile_bff.Tests.Infrastructure.Resilience;

public sealed class RetryPolicyTests
{
    [Fact]
    public async Task GetRetryPolicy_WithSuccessfulResponse_DoesNotRetry()
    {
        var options = new HttpClientPolicyOptions
        {
            RetryCount = 3,
            RetryBaseDelaySeconds = 0
        };
        var policy = RetryPolicy.GetRetryPolicy(options);
        var attemptCount = 0;

        var result = await policy.ExecuteAsync(() =>
        {
            attemptCount++;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        });

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(1, attemptCount);
    }

    [Fact]
    public async Task GetRetryPolicy_WithTooManyRequests_RetriesUntilResponseSucceeds()
    {
        var options = new HttpClientPolicyOptions
        {
            RetryCount = 3,
            RetryBaseDelaySeconds = 0
        };
        var policy = RetryPolicy.GetRetryPolicy(options);
        var attemptCount = 0;

        var result = await policy.ExecuteAsync(() =>
        {
            attemptCount++;

            return Task.FromResult(new HttpResponseMessage(
                attemptCount < 3 ? HttpStatusCode.TooManyRequests : HttpStatusCode.OK));
        });

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(3, attemptCount);
    }

    [Fact]
    public async Task GetRetryPolicy_WithTransientHttpRequestException_RetriesUntilExecutionSucceeds()
    {
        var options = new HttpClientPolicyOptions
        {
            RetryCount = 3,
            RetryBaseDelaySeconds = 0
        };
        var policy = RetryPolicy.GetRetryPolicy(options);
        var attemptCount = 0;

        var result = await policy.ExecuteAsync(() =>
        {
            attemptCount++;

            if (attemptCount < 3)
                throw new HttpRequestException("transient failure");

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        });

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(3, attemptCount);
    }
}
