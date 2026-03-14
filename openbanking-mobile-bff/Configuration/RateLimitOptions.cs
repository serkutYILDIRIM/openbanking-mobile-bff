namespace openbanking_mobile_bff.Configuration;

public sealed class RateLimitOptions
{
    public int PermitLimit { get; set; }
    public int WindowSeconds { get; set; }
    public int QueueLimit { get; set; }
}

