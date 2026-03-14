using System.Net;
using System.Text.Json;
using openbanking_mobile_bff.Common.Exceptions;

namespace openbanking_mobile_bff.Common.Middleware;

public sealed class GlobalExceptionMiddleware : IMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (OhvpsException ex)
        {
            context.Response.StatusCode = (int)ex.StatusCode;
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                Id = context.TraceIdentifier,
                Path = context.Request.Path,
                Timestamp = DateTime.UtcNow,
                HttpCode = ex.StatusCode,
                ErrorCode = ex.ErrorCode,
                ErrorMessage = ex.ErrorMessage
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
        }
        catch (Exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                Id = context.TraceIdentifier,
                Path = context.Request.Path,
                Timestamp = DateTime.UtcNow,
                HttpCode = HttpStatusCode.InternalServerError,
                ErrorCode = "TR.OHVPS.Server.InternalError",
                ErrorMessage = "An unexpected error occurred."
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
        }
    }
}

