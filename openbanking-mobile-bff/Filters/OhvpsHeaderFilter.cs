using Microsoft.AspNetCore.Mvc.Filters;
using openbanking_mobile_bff.Common.Constants;

namespace openbanking_mobile_bff.Filters;

public sealed class OhvpsHeaderFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var result = await next();

        if (context.HttpContext.Request.Headers.TryGetValue(OhvpsConstants.RequestIdHeader, out var requestId))
        {
            context.HttpContext.Response.Headers[OhvpsConstants.RequestIdHeader] = requestId;
        }
    }
}

