using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using openbanking_mobile_bff.Common.Constants;
using openbanking_mobile_bff.Common.Exceptions;
using openbanking_mobile_bff.Common.Middleware;

namespace openbanking_mobile_bff.Tests.Common.Middleware;

public sealed class OhvpsHeaderValidationMiddlewareTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    [Fact]
    public async Task InvokeAsync_WithNonApiPath_CallsNextWithoutValidatingHeaders()
    {
        var context = new DefaultHttpContext();
        context.Request.Path = "/health";
        
        var nextCalled = false;

        await new OhvpsHeaderValidationMiddleware().InvokeAsync(context, _ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });

        Assert.True(nextCalled);
        Assert.Equal(200, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_WithApiPathAndAllRequiredHeaders_CallsNext()
    {
        var context = new DefaultHttpContext();
        context.Request.Path = "/api/accounts";
        context.Request.Headers[OhvpsConstants.RequestIdHeader] = "req-123";
        context.Request.Headers[OhvpsConstants.AspspCodeHeader] = "aspsp-001";
        context.Request.Headers[OhvpsConstants.TppCodeHeader] = "tpp-001";
        
        var nextCalled = false;

        await new OhvpsHeaderValidationMiddleware().InvokeAsync(context, _ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });

        Assert.True(nextCalled);
        Assert.Equal(200, context.Response.StatusCode);
    }

    [Theory]
    [InlineData(OhvpsConstants.RequestIdHeader)]
    [InlineData(OhvpsConstants.AspspCodeHeader)]
    [InlineData(OhvpsConstants.TppCodeHeader)]
    public async Task InvokeAsync_WithApiPathAndMissingRequiredHeader_ReturnsBadRequestAndDoesNotCallNext(string missingHeader)
    {
        var context = new DefaultHttpContext();
        
        context.Request.Path = "/api/accounts";
        context.Request.Headers[OhvpsConstants.RequestIdHeader] = "req-123";
        context.Request.Headers[OhvpsConstants.AspspCodeHeader] = "aspsp-001";
        context.Request.Headers[OhvpsConstants.TppCodeHeader] = "tpp-001";
        context.Request.Headers.Remove(missingHeader);
        
        var nextCalled = false;

        await new OhvpsHeaderValidationMiddleware().InvokeAsync(context, _ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        });

        Assert.False(nextCalled);
        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        Assert.Equal("application/json", context.Response.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_WithMissingHeaders_WritesErrorResponseBodyWithExpectedFields()
    {
        var context = new DefaultHttpContext();
        context.TraceIdentifier = "trace-1";
        context.Request.Path = "/api/accounts";
        context.Response.Body = new MemoryStream();

        await new OhvpsHeaderValidationMiddleware().InvokeAsync(context, _ => Task.CompletedTask);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var error = JsonSerializer.Deserialize<ErrorResponse>(body, JsonOptions);

        Assert.NotNull(error);
        Assert.Equal("trace-1", error!.Id);
        Assert.Equal("/api/accounts", error.Path);
        Assert.Equal(HttpStatusCode.BadRequest, error.HttpCode);
        Assert.Equal("TR.OHVPS.Resource.MissingHeader", error.ErrorCode);
        Assert.Equal("Required OHVPS headers are missing.", error.ErrorMessage);
    }
}
