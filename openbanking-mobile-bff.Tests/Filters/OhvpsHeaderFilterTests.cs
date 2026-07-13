using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using openbanking_mobile_bff.Common.Constants;
using openbanking_mobile_bff.Filters;

namespace openbanking_mobile_bff.Tests.Filters;

public sealed class OhvpsHeaderFilterTests
{
    [Fact]
    public async Task OnActionExecutionAsync_WithRequestIdHeader_CopiesHeaderToResponse()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers[OhvpsConstants.RequestIdHeader] = "req-123";

        var context = new ActionExecutingContext(
            new ActionContext(httpContext, new RouteData(), new ActionDescriptor()),
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            new object());

        var nextCalled = false;

        await new OhvpsHeaderFilter().OnActionExecutionAsync(context, () =>
        {
            nextCalled = true;
            return Task.FromResult(new ActionExecutedContext(
                new ActionContext(httpContext, new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>(),
                new object()));
        });

        Assert.True(nextCalled);
        Assert.True(httpContext.Response.Headers.ContainsKey(OhvpsConstants.RequestIdHeader));
        Assert.Equal("req-123", httpContext.Response.Headers[OhvpsConstants.RequestIdHeader].ToString());
    }

    [Fact]
    public async Task OnActionExecutionAsync_WithoutRequestIdHeader_DoesNotAddHeaderToResponse()
    {
        var httpContext = new DefaultHttpContext();

        var context = new ActionExecutingContext(
            new ActionContext(httpContext, new RouteData(), new ActionDescriptor()),
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            new object());

        var nextCalled = false;

        await new OhvpsHeaderFilter().OnActionExecutionAsync(context, () =>
        {
            nextCalled = true;
            return Task.FromResult(new ActionExecutedContext(
                new ActionContext(httpContext, new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>(),
                new object()));
        });

        Assert.True(nextCalled);
        Assert.False(httpContext.Response.Headers.ContainsKey(OhvpsConstants.RequestIdHeader));
    }
}
