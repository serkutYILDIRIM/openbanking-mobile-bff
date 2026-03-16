﻿﻿using openbanking_mobile_bff.Common.Middleware;
using openbanking_mobile_bff.Configuration;
using openbanking_mobile_bff.Domain.Account.Services;
using openbanking_mobile_bff.Domain.Card.Services;
using openbanking_mobile_bff.Domain.Consent.Services;
using openbanking_mobile_bff.Domain.Gkd.Services;
using openbanking_mobile_bff.Domain.Payment.Services;
using openbanking_mobile_bff.Filters;
using openbanking_mobile_bff.Infrastructure.Cache;

namespace openbanking_mobile_bff.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBffServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MicroserviceEndpointOptions>(
            configuration.GetSection("MicroserviceEndpoints"));
        services.Configure<HttpClientPolicyOptions>(
            configuration.GetSection("HttpClientPolicy"));
        services.Configure<RateLimitOptions>(
            configuration.GetSection("RateLimit"));
        services.Configure<CacheOptions>(
            configuration.GetSection("Cache"));
        services.Configure<JwtValidationOptions>(
            configuration.GetSection("JwtValidation"));
        services.Configure<OhvpsHeaderOptions>(
            configuration.GetSection("OhvpsHeaders"));
        services.Configure<SwaggerOptions>(
            configuration.GetSection("Swagger"));
        services.Configure<HhsApiPathOptions>(
            configuration.GetSection("HhsApiPaths"));
        services.Configure<BffRoleOptions>(
            configuration.GetSection("BffRole"));

        services.AddTransient<GlobalExceptionMiddleware>();
        services.AddTransient<RequestLoggingMiddleware>();
        services.AddTransient<OhvpsHeaderValidationMiddleware>();
        services.AddTransient<JwtBearerMiddleware>();
        services.AddTransient<JwsValidationMiddleware>();

        services.AddScoped<OhvpsHeaderFilter>();
        services.AddScoped<IdempotencyFilter>();
        services.AddScoped<ValidationFilter>();

        services.AddScoped<ICacheService, RedisCacheService>();

        var cacheOptions = configuration
            .GetSection("Cache")
            .Get<CacheOptions>() ?? new CacheOptions();

        if (!string.IsNullOrEmpty(cacheOptions.RedisConnectionString))
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = cacheOptions.RedisConnectionString;
            });
        }
        else
        {
            services.AddDistributedMemoryCache();
        }

        services.AddControllers(options =>
        {
            options.Filters.Add<OhvpsHeaderFilter>();
            options.Filters.Add<ValidationFilter>();
        });

        services.AddBffAuthentication(configuration);
        services.AddBffHttpClients(configuration);
        services.AddBffSwagger(configuration);
        services.AddBffRateLimiting(configuration);
        services.AddBffHealthChecks();

        services.AddScoped<IConsentService, ConsentService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<ICardService, CardService>();
        services.AddScoped<IGkdProxyService, GkdProxyService>();

        return services;
    }
}

