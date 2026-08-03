using System.Collections;
using System.Reflection;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using openbanking_mobile_bff.Extensions;

namespace openbanking_mobile_bff.Tests.Extensions;

public sealed class RateLimitExtensionsTests
{
    [Fact]
    public void AddBffRateLimiting_WithConfiguredOptions_AppliesConfiguredPermitLimitAndRejectionStatusCode()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["RateLimit:PermitLimit"] = "2",
                ["RateLimit:WindowSeconds"] = "60",
                ["RateLimit:QueueLimit"] = "0"
            })
            .Build();

        services.AddBffRateLimiting(configuration);

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<RateLimiterOptions>>().Value;
        using var limiter = CreateSlidingLimiter(options);

        using var firstLease = limiter.AttemptAcquire(1);
        using var secondLease = limiter.AttemptAcquire(1);
        using var thirdLease = limiter.AttemptAcquire(1);

        Assert.Equal(StatusCodes.Status429TooManyRequests, options.RejectionStatusCode);
        Assert.True(firstLease.IsAcquired is true);
        Assert.True(secondLease.IsAcquired is true);
        Assert.False(thirdLease.IsAcquired is true);
    }

    [Fact]
    public void AddBffRateLimiting_WithoutConfiguredOptions_UsesDefaultPermitLimit()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        services.AddBffRateLimiting(configuration);

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<RateLimiterOptions>>().Value;
        using var limiter = CreateSlidingLimiter(options);

        for (var i = 0; i < 100; i++)
        {
            using var lease = limiter.AttemptAcquire(1);
            Assert.True(lease.IsAcquired is true);
        }

        using var overflowLease = limiter.AttemptAcquire(1);

        Assert.False(overflowLease.IsAcquired is true);
    }

    private static RateLimiter CreateSlidingLimiter(RateLimiterOptions options)
    {
        var policyMapMember = typeof(RateLimiterOptions)
            .GetMember("PolicyMap", BindingFlags.Instance | BindingFlags.NonPublic)
            .Single();

        var policyMap = policyMapMember switch
        {
            PropertyInfo property => property.GetValue(options) as IDictionary,
            FieldInfo field => field.GetValue(options) as IDictionary,
            _ => null
        };

        Assert.NotNull(policyMap);
        Assert.True(policyMap!.Contains("sliding"));

        var policy = policyMap["sliding"];
        Assert.NotNull(policy);

        var partitioner = policy!.GetType()
            .GetProperty("Partitioner", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?
            .GetValue(policy) as Delegate;

        Assert.NotNull(partitioner);

        var partition = partitioner!.DynamicInvoke(new DefaultHttpContext());
        
        Assert.NotNull(partition);

        var factory = partition!.GetType()
            .GetProperty("Factory", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?
            .GetValue(partition) as Delegate;

        Assert.NotNull(factory);

        var keyType = factory!.Method.GetParameters().Single().ParameterType;
        var key = keyType == typeof(string) ? "test-key" : Activator.CreateInstance(keyType);
        var limiter = factory.DynamicInvoke(key) as RateLimiter;

        return Assert.IsType<SlidingWindowRateLimiter>(limiter);
    }
}
