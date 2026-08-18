using Microsoft.AspNetCore.Http;
using openbanking_mobile_bff.Common.Middleware;

namespace openbanking_mobile_bff.Tests.Common.Middleware;

public sealed class JwtBearerMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_CallsNextWithSameContext()
    {
        var middleware = new JwtBearerMiddleware();
        var context = new DefaultHttpContext();
        HttpContext? capturedContext = null;
        var nextCalled = false;

        await middleware.InvokeAsync(context, ctx =>
        {
            nextCalled = true;
            capturedContext = ctx;
            ctx.Response.StatusCode = StatusCodes.Status204NoContent;
            return Task.CompletedTask;
        });

        Assert.True(nextCalled);
        Assert.Same(context, capturedContext);
        Assert.Equal(StatusCodes.Status204NoContent, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_WhenNextThrows_PropagatesException()
    {
        var middleware = new JwtBearerMiddleware();
        var context = new DefaultHttpContext();
        var exception = new InvalidOperationException("next failed");

        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            middleware.InvokeAsync(context, _ => Task.FromException(exception)));

        Assert.Same(exception, thrown);
    }
}

