using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using openbanking_mobile_bff.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace openbanking_mobile_bff.Tests.Extensions;

public sealed class SwaggerExtensionsTests
{
    [Fact]
    public void AddBffSwagger_WithConfiguredSwaggerOptions_RegistersSwaggerGeneratorOptions()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IWebHostEnvironment>(new TestWebHostEnvironment());

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Swagger:Title"] = "Mobile API",
                ["Swagger:Version"] = "v9",
                ["Swagger:Description"] = "Mobile BFF Swagger"
            })
            .Build();

        services.AddBffSwagger(configuration);

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<SwaggerGeneratorOptions>>().Value;

        Assert.True(options.SwaggerDocs.ContainsKey("v9"));

        var swaggerDoc = options.SwaggerDocs["v9"];
        Assert.Equal("Mobile API", swaggerDoc.Title);
        Assert.Equal("v9", swaggerDoc.Version);
        Assert.Equal("Mobile BFF Swagger", swaggerDoc.Description);

        Assert.True(options.SecuritySchemes.ContainsKey(JwtBearerDefaults.AuthenticationScheme));

        var securityScheme = options.SecuritySchemes[JwtBearerDefaults.AuthenticationScheme];
        Assert.Equal(SecuritySchemeType.Http, securityScheme.Type);
        Assert.Equal(JwtBearerDefaults.AuthenticationScheme, securityScheme.Scheme);
        Assert.Equal("JWT", securityScheme.BearerFormat);

        var securityRequirementFactory = Assert.Single(options.SecurityRequirements);
        var securityRequirement = securityRequirementFactory(new OpenApiDocument());
        var requirementScheme = Assert.Single(securityRequirement.Keys);
        Assert.Equal(JwtBearerDefaults.AuthenticationScheme, requirementScheme.Reference.Id);
    }

    [Fact]
    public void AddBffSwagger_WithMissingSwaggerOptions_UsesDefaultValues()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IWebHostEnvironment>(new TestWebHostEnvironment());

        var configuration = new ConfigurationBuilder().Build();

        services.AddBffSwagger(configuration);

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<SwaggerGeneratorOptions>>().Value;

        Assert.True(options.SwaggerDocs.ContainsKey("v1"));

        var swaggerDoc = options.SwaggerDocs["v1"];
        Assert.Equal("OpenBanking Mobile BFF", swaggerDoc.Title);
        Assert.Equal("v1", swaggerDoc.Version);
        Assert.Equal("OHVPS Mobile BFF API", swaggerDoc.Description);
    }

    private sealed class TestWebHostEnvironment : IWebHostEnvironment
    {
        public string ApplicationName { get; set; } = "openbanking-mobile-bff.Tests";

        public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();

        public string WebRootPath { get; set; } = string.Empty;

        public string EnvironmentName { get; set; } = "Development";

        public string ContentRootPath { get; set; } = string.Empty;

        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}




