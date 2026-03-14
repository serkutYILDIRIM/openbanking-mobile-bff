namespace openbanking_mobile_bff.Common.Middleware;

public sealed class JwsValidationMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await next(context);
    }
}

