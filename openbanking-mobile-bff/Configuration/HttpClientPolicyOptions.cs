namespace openbanking_mobile_bff.Configuration;

public sealed class HttpClientPolicyOptions
{
    public int RetryCount { get; set; }
    public int RetryBaseDelaySeconds { get; set; }
    public int CircuitBreakerThreshold { get; set; }
    public int CircuitBreakerDurationSeconds { get; set; }
    public int TimeoutSeconds { get; set; }
}

