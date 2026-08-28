using openbanking_mobile_bff.Configuration;

namespace openbanking_mobile_bff.Tests.Configuration;

public sealed class HttpClientPolicyOptionsTests
{
    [Fact]
    public void Constructor_Always_InitializesDefaultValues()
    {
        var options = new HttpClientPolicyOptions();

        Assert.Equal(0, options.RetryCount);
        Assert.Equal(0, options.RetryBaseDelaySeconds);
        Assert.Equal(0, options.CircuitBreakerThreshold);
        Assert.Equal(0, options.CircuitBreakerDurationSeconds);
        Assert.Equal(0, options.TimeoutSeconds);
    }

    [Fact]
    public void Properties_WithProvidedValues_PreservesAssignedState()
    {
        var options = new HttpClientPolicyOptions
        {
            RetryCount = 3,
            RetryBaseDelaySeconds = 2,
            CircuitBreakerThreshold = 5,
            CircuitBreakerDurationSeconds = 60,
            TimeoutSeconds = 30
        };

        Assert.Equal(3, options.RetryCount);
        Assert.Equal(2, options.RetryBaseDelaySeconds);
        Assert.Equal(5, options.CircuitBreakerThreshold);
        Assert.Equal(60, options.CircuitBreakerDurationSeconds);
        Assert.Equal(30, options.TimeoutSeconds);
    }

    [Theory]
    [InlineData(0, 0, 0, 0, 0)]
    [InlineData(-1, -1, -1, -1, -1)]
    [InlineData(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue)]
    public void Properties_WithEdgeValues_PreservesAssignedState(
        int retryCount,
        int retryBaseDelaySeconds,
        int circuitBreakerThreshold,
        int circuitBreakerDurationSeconds,
        int timeoutSeconds)
    {
        var options = new HttpClientPolicyOptions
        {
            RetryCount = retryCount,
            RetryBaseDelaySeconds = retryBaseDelaySeconds,
            CircuitBreakerThreshold = circuitBreakerThreshold,
            CircuitBreakerDurationSeconds = circuitBreakerDurationSeconds,
            TimeoutSeconds = timeoutSeconds
        };

        Assert.Equal(retryCount, options.RetryCount);
        Assert.Equal(retryBaseDelaySeconds, options.RetryBaseDelaySeconds);
        Assert.Equal(circuitBreakerThreshold, options.CircuitBreakerThreshold);
        Assert.Equal(circuitBreakerDurationSeconds, options.CircuitBreakerDurationSeconds);
        Assert.Equal(timeoutSeconds, options.TimeoutSeconds);
    }
}

