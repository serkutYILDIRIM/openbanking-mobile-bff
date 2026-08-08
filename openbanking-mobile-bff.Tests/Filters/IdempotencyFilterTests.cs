using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using openbanking_mobile_bff.Filters;

namespace openbanking_mobile_bff.Tests.Filters;

public sealed class IdempotencyFilterTests
{
    [Fact]
    public async Task OnActionExecutionAsync_WithValidContext_CallsNext()
    {
        var httpContext = new DefaultHttpContext();

        var context = new ActionExecutingContext(
            new ActionContext(httpContext, new RouteData(), new ActionDescriptor()),
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            new object());

        var nextCalled = false;

        await new IdempotencyFilter().OnActionExecutionAsync(context, () =>
        {
            nextCalled = true;
            return Task.FromResult(new ActionExecutedContext(
                new ActionContext(httpContext, new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>(),
                new object()));
        });

        Assert.True(nextCalled);
    }

    [Fact]
    public async Task OnActionExecutionAsync_WhenNextThrows_PropagatesException()
    {
        var httpContext = new DefaultHttpContext();

        var context = new ActionExecutingContext(
            new ActionContext(httpContext, new RouteData(), new ActionDescriptor()),
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            new object());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new IdempotencyFilter().OnActionExecutionAsync(context, () =>
                throw new InvalidOperationException("Expected exception.")));
    }
}

