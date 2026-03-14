using System.Net;
using System.Text.Json;
using openbanking_mobile_bff.Common.Constants;
using openbanking_mobile_bff.Common.Exceptions;

namespace openbanking_mobile_bff.Common.Middleware;

public sealed class OhvpsHeaderValidationMiddleware : IMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            if (!context.Request.Headers.ContainsKey(OhvpsConstants.RequestIdHeader) ||
                !context.Request.Headers.ContainsKey(OhvpsConstants.AspspCodeHeader) ||
                !context.Request.Headers.ContainsKey(OhvpsConstants.TppCodeHeader))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var response = new ErrorResponse
                {
                    Id = context.TraceIdentifier,
                    Path = context.Request.Path,
                    Timestamp = DateTime.UtcNow,
                    HttpCode = HttpStatusCode.BadRequest,
                    ErrorCode = "TR.OHVPS.Resource.MissingHeader",
                    ErrorMessage = "Required OHVPS headers are missing."
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
                return;
            }
        }

        await next(context);
    }
}

