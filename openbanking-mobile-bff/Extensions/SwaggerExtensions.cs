using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;
using openbanking_mobile_bff.Configuration;

namespace openbanking_mobile_bff.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddBffSwagger(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var swaggerOptions = configuration
            .GetSection("Swagger")
            .Get<SwaggerOptions>() ?? new SwaggerOptions
            {
                Title = "OpenBanking Mobile BFF",
                Version = "v1",
                Description = "OHVPS Mobile BFF API"
            };

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(swaggerOptions.Version, new OpenApiInfo
            {
                Title = swaggerOptions.Title,
                Version = swaggerOptions.Version,
                Description = swaggerOptions.Description
            });

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter JWT Bearer token"
            });

            options.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference(JwtBearerDefaults.AuthenticationScheme)] = new List<string>()
            });
        });

        return services;
    }

    public static WebApplication UseBffSwagger(this WebApplication app)
    {
        var swaggerOptions = app.Configuration
            .GetSection("Swagger")
            .Get<SwaggerOptions>() ?? new SwaggerOptions { Version = "v1", Title = "OpenBanking Mobile BFF" };

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/swagger/{swaggerOptions.Version}/swagger.json", swaggerOptions.Title);
        });

        return app;
    }
}
