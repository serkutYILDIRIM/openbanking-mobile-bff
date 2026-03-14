using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using openbanking_mobile_bff.Common.Exceptions;

namespace openbanking_mobile_bff.Filters;

public sealed class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var fieldErrors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors.Select(e => new FieldError
                {
                    Field = x.Key,
                    Message = e.ErrorMessage
                }))
                .ToList();

            var errorResponse = new ErrorResponse
            {
                Id = context.HttpContext.TraceIdentifier,
                Path = context.HttpContext.Request.Path,
                Timestamp = DateTime.UtcNow,
                HttpCode = HttpStatusCode.BadRequest,
                ErrorCode = "TR.OHVPS.Resource.ValidationError",
                ErrorMessage = "One or more validation errors occurred.",
                FieldErrors = fieldErrors
            };

            context.Result = new BadRequestObjectResult(errorResponse);
            return;
        }

        await next();
    }
}

