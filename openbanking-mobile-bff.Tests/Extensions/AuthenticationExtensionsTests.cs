using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using openbanking_mobile_bff.Extensions;

namespace openbanking_mobile_bff.Tests.Extensions;

public sealed class AuthenticationExtensionsTests
{
    [Fact]
    public void AddBffAuthentication_WithConfiguredJwtValidationOptions_RegistersJwtBearerOptions()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JwtValidation:Issuer"] = "https://issuer.local",
                ["JwtValidation:Audience"] = "mobile-bff",
                ["JwtValidation:JwksUri"] = "https://issuer.local/.well-known/jwks.json",
                ["JwtValidation:ValidateLifetime"] = "true"
            })
            .Build();

        services.AddBffAuthentication(configuration);

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>()
            .Get(JwtBearerDefaults.AuthenticationScheme);

        Assert.Equal("https://issuer.local", options.Authority);
        Assert.Equal("mobile-bff", options.Audience);
        Assert.Equal("https://issuer.local/.well-known/jwks.json", options.MetadataAddress);
        Assert.NotNull(options.TokenValidationParameters);
        Assert.True(options.TokenValidationParameters!.ValidateLifetime);
        Assert.True(options.TokenValidationParameters.ValidateIssuer);
        Assert.Equal("https://issuer.local", options.TokenValidationParameters.ValidIssuer);
        Assert.True(options.TokenValidationParameters.ValidateAudience);
        Assert.Equal("mobile-bff", options.TokenValidationParameters.ValidAudience);
    }

    [Fact]
    public void AddBffAuthentication_WithMissingJwtValidationOptions_UsesDefaultEmptyValues()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        services.AddBffAuthentication(configuration);

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>()
            .Get(JwtBearerDefaults.AuthenticationScheme);

        Assert.Equal(string.Empty, options.Authority);
        Assert.Equal(string.Empty, options.Audience);
        Assert.Equal(string.Empty, options.MetadataAddress);
        Assert.NotNull(options.TokenValidationParameters);
        Assert.False(options.TokenValidationParameters!.ValidateIssuer);
        Assert.Equal(string.Empty, options.TokenValidationParameters.ValidIssuer);
        Assert.False(options.TokenValidationParameters.ValidateAudience);
        Assert.Equal(string.Empty, options.TokenValidationParameters.ValidAudience);
    }
}
