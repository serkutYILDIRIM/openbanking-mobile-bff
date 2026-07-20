using System.Net;
using openbanking_mobile_bff.Configuration;
using openbanking_mobile_bff.Infrastructure.Resilience;

namespace openbanking_mobile_bff.Tests.Infrastructure.Resilience;

public sealed class TimeoutPolicyTests
{
    [Fact]
    public async Task GetTimeoutPolicy_WithOperationInsideTimeout_CompletesSuccessfully()
    {
        var options = new HttpClientPolicyOptions
        {
            TimeoutSeconds = 1
        };
        var policy = TimeoutPolicy.GetTimeoutPolicy(options);

        var result = await policy.ExecuteAsync(async cancellationToken =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(50), cancellationToken);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }, CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task GetTimeoutPolicy_WithOperationExceedingTimeout_ThrowsTimeoutRejectedException()
    {
        var options = new HttpClientPolicyOptions
        {
            TimeoutSeconds = 1
        };
        var policy = TimeoutPolicy.GetTimeoutPolicy(options);

        await Assert.ThrowsAsync<Polly.Timeout.TimeoutRejectedException>(() =>
            policy.ExecuteAsync(async cancellationToken =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }, CancellationToken.None));
    }
}