using Microsoft.AspNetCore.Mvc.Filters;
using openbanking_mobile_bff.Common.Constants;

namespace openbanking_mobile_bff.Filters;

public sealed class IdempotencyFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await next();
    }
}

