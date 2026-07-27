using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using openbanking_mobile_bff.Extensions;

namespace openbanking_mobile_bff.Tests.Extensions;

public sealed class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddBffServices_WithRedisConnectionString_RegistersRedisDistributedCache()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Cache:RedisConnectionString"] = "localhost:6379"
            })
            .Build();

        services.AddBffServices(configuration);

        using var provider = services.BuildServiceProvider();
        var cache = provider.GetRequiredService<IDistributedCache>();

        Assert.Equal("RedisCacheImpl", cache.GetType().Name);
    }

    [Fact]
    public void AddBffServices_WithoutRedisConnectionString_RegistersMemoryDistributedCache()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        services.AddBffServices(configuration);

        using var provider = services.BuildServiceProvider();
        var cache = provider.GetRequiredService<IDistributedCache>();

        Assert.Equal("MemoryDistributedCache", cache.GetType().Name);
    }
}