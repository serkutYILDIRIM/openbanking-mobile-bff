namespace openbanking_mobile_bff.Configuration;

public sealed class CacheOptions
{
    public string RedisConnectionString { get; set; } = string.Empty;
    public int DefaultExpirationMinutes { get; set; }
}

