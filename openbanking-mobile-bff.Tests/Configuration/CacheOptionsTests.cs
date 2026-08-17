using openbanking_mobile_bff.Configuration;

namespace openbanking_mobile_bff.Tests.Configuration;

public sealed class CacheOptionsTests
{
    [Fact]
    public void Constructor_Always_InitializesDefaultValues()
    {
        var options = new CacheOptions();

        Assert.Equal(string.Empty, options.RedisConnectionString);
        Assert.Equal(0, options.DefaultExpirationMinutes);
    }

    [Fact]
    public void Properties_WithProvidedValues_PreservesAssignedState()
    {
        var options = new CacheOptions
        {
            RedisConnectionString = "localhost:6379,password=secret",
            DefaultExpirationMinutes = 15
        };

        Assert.Equal("localhost:6379,password=secret", options.RedisConnectionString);
        Assert.Equal(15, options.DefaultExpirationMinutes);
    }

    [Fact]
    public void Properties_WithEdgeValues_PreservesAssignedState()
    {
        var options = new CacheOptions
        {
            RedisConnectionString = string.Empty,
            DefaultExpirationMinutes = -1
        };
        Assert.Equal(string.Empty, options.RedisConnectionString);
        Assert.Equal(-1, options.DefaultExpirationMinutes);
    }
}

