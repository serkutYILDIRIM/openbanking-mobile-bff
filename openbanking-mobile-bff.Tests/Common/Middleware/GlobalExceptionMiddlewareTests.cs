using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using openbanking_mobile_bff.Common.Exceptions;
using openbanking_mobile_bff.Common.Middleware;

namespace openbanking_mobile_bff.Tests.Common.Middleware;

public sealed class GlobalExceptionMiddlewareTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    [Fact]
    public async Task InvokeAsync_WithNoException_CallsNext()
    {
        var context = new DefaultHttpContext();        
        var nextCalled = false;

        await new GlobalExceptionMiddleware().InvokeAsync(context, _ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });

        Assert.True(nextCalled);
        Assert.Equal(200, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_WithOhvpsException_WritesCustomErrorPayload()
    {
        var context = new DefaultHttpContext();
        
        context.TraceIdentifier = "trace-1";
        context.Request.Path = "/api/payments";
        
        context.Response.Body = new MemoryStream();
        var before = DateTime.UtcNow;

        await new GlobalExceptionMiddleware().InvokeAsync(
            context,
            _ => throw new OhvpsException(HttpStatusCode.BadRequest, "TR.OHVPS.Validation.InvalidInput", "Input is invalid."));

        var after = DateTime.UtcNow;
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var error = JsonSerializer.Deserialize<ErrorResponse>(body, JsonOptions);

        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        Assert.Equal("application/json", context.Response.ContentType);
        Assert.NotNull(error);
        Assert.Equal("trace-1", error!.Id);
        Assert.Equal("/api/payments", error.Path);
        
        Assert.Equal(HttpStatusCode.BadRequest, error.HttpCode);
        Assert.Equal("TR.OHVPS.Validation.InvalidInput", error.ErrorCode);
        Assert.Equal("Input is invalid.", error.ErrorMessage);
        Assert.InRange(error.Timestamp, before, after);
    }

    [Fact]
    public async Task InvokeAsync_WithUnexpectedException_WritesInternalServerErrorPayload()
    {
        var context = new DefaultHttpContext();
        context.TraceIdentifier = "trace-2";
        context.Request.Path = "/api/cards";
        context.Response.Body = new MemoryStream();
        var before = DateTime.UtcNow;

        await new GlobalExceptionMiddleware().InvokeAsync(
            context,
            _ => throw new InvalidOperationException("Unexpected failure"));

        var after = DateTime.UtcNow;
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var error = JsonSerializer.Deserialize<ErrorResponse>(body, JsonOptions);

        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        Assert.Equal("application/json", context.Response.ContentType);
        Assert.NotNull(error);
        Assert.Equal("trace-2", error!.Id);
        Assert.Equal("/api/cards", error.Path);
        Assert.Equal(HttpStatusCode.InternalServerError, error.HttpCode);
        Assert.Equal("TR.OHVPS.Server.InternalError", error.ErrorCode);
        Assert.Equal("An unexpected error occurred.", error.ErrorMessage);
        Assert.InRange(error.Timestamp, before, after);
    }
}
