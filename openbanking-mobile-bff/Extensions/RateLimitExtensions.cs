using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using openbanking_mobile_bff.Configuration;

namespace openbanking_mobile_bff.Extensions;

public static class RateLimitExtensions
{
    public static IServiceCollection AddBffRateLimiting(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var rateLimitOptions = configuration
            .GetSection("RateLimit")
            .Get<RateLimitOptions>() ?? new RateLimitOptions
            {
                PermitLimit = 100,
                WindowSeconds = 60,
                QueueLimit = 10
            };

        services.AddRateLimiter(options =>
        {
            options.AddSlidingWindowLimiter("sliding", limiterOptions =>
            {
                limiterOptions.PermitLimit = rateLimitOptions.PermitLimit;
                limiterOptions.Window = TimeSpan.FromSeconds(rateLimitOptions.WindowSeconds);
                limiterOptions.QueueLimit = rateLimitOptions.QueueLimit;
                limiterOptions.SegmentsPerWindow = 4;
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        return services;
    }
}
