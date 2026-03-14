using Microsoft.AspNetCore.Authentication.JwtBearer;
using openbanking_mobile_bff.Configuration;

namespace openbanking_mobile_bff.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddBffAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtOptions = configuration
            .GetSection("JwtValidation")
            .Get<JwtValidationOptions>() ?? new JwtValidationOptions();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = jwtOptions.Issuer;
                options.Audience = jwtOptions.Audience;
                options.MetadataAddress = jwtOptions.JwksUri;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateLifetime = jwtOptions.ValidateLifetime,
                    ValidateIssuer = !string.IsNullOrEmpty(jwtOptions.Issuer),
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = !string.IsNullOrEmpty(jwtOptions.Audience),
                    ValidAudience = jwtOptions.Audience
                };
            });

        services.AddAuthorization();

        return services;
    }
}

