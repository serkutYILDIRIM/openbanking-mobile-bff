using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using openbanking_mobile_bff.Extensions;

namespace openbanking_mobile_bff.Tests.Extensions;

public sealed class HealthCheckExtensionsTests
{
    [Fact]
    public void AddBffHealthChecks_RegistersSelfHealthCheck()
    {
        var services = new ServiceCollection();

        services.AddBffHealthChecks();

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<HealthCheckServiceOptions>>().Value;

        var registration = Assert.Single(options.Registrations);
        Assert.Equal("self", registration.Name);
    }

}
