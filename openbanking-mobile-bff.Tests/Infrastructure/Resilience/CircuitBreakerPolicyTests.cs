using System.Net;
using openbanking_mobile_bff.Configuration;
using openbanking_mobile_bff.Infrastructure.Resilience;

namespace openbanking_mobile_bff.Tests.Infrastructure.Resilience;

public sealed class CircuitBreakerPolicyTests
{
    [Fact]
    public async Task GetCircuitBreakerPolicy_WithConsecutiveTransientResponses_OpensCircuitAndRejectsNextCall()
    {
        var options = new HttpClientPolicyOptions
        {
            CircuitBreakerThreshold = 2,
            CircuitBreakerDurationSeconds = 5
        };
        var policy = CircuitBreakerPolicy.GetCircuitBreakerPolicy(options);

        await policy.ExecuteAsync(() =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)));

        await policy.ExecuteAsync(() =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)));

        await Assert.ThrowsAsync<Polly.CircuitBreaker.BrokenCircuitException>(() =>
            policy.ExecuteAsync(() =>
                Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK))));
    }

    [Fact]
    public async Task GetCircuitBreakerPolicy_WithTransientException_OpensCircuitAndRejectsSubsequentCall()
    {
        var options = new HttpClientPolicyOptions
        {
            CircuitBreakerThreshold = 1,
            CircuitBreakerDurationSeconds = 5
        };
        var policy = CircuitBreakerPolicy.GetCircuitBreakerPolicy(options);

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            policy.ExecuteAsync(() => throw new HttpRequestException("transient failure")));

        await Assert.ThrowsAsync<Polly.CircuitBreaker.BrokenCircuitException>(() =>
            policy.ExecuteAsync(() =>
                Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK))));
    }
}
