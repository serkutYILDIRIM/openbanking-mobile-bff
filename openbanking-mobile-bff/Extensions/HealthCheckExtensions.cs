using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace openbanking_mobile_bff.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddBffHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy());

        return services;
    }

    public static WebApplication MapBffHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health");
        return app;
    }
}

