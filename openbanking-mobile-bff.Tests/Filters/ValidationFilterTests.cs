using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using openbanking_mobile_bff.Common.Exceptions;
using openbanking_mobile_bff.Filters;

namespace openbanking_mobile_bff.Tests.Filters;

public sealed class ValidationFilterTests
{
    [Fact]
    public async Task OnActionExecutionAsync_WithInvalidModelState_ReturnsBadRequestErrorResponse()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.TraceIdentifier = "trace-1";
        httpContext.Request.Path = "/api/accounts";

        var context = new ActionExecutingContext(
            new ActionContext(httpContext, new RouteData(), new ActionDescriptor()),
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            new object());

        context.ModelState.AddModelError("accountNumber", "Account number is required.");
        var before = DateTime.UtcNow;

        await new ValidationFilter().OnActionExecutionAsync(context, () =>
            throw new InvalidOperationException("Next should not be called for invalid model state."));

        var after = DateTime.UtcNow;

        var result = Assert.IsType<BadRequestObjectResult>(context.Result);
        var error = Assert.IsType<ErrorResponse>(result.Value);

        Assert.Equal("trace-1", error.Id);
        Assert.Equal("/api/accounts", error.Path);
        Assert.Equal(HttpStatusCode.BadRequest, error.HttpCode);
        Assert.Equal("TR.OHVPS.Resource.ValidationError", error.ErrorCode);
        Assert.Equal("One or more validation errors occurred.", error.ErrorMessage);
        Assert.Single(error.FieldErrors);

        var fieldError = Assert.Single(error.FieldErrors);
        Assert.Equal("accountNumber", fieldError.Field);
        Assert.Equal("Account number is required.", fieldError.Message);
        Assert.InRange(error.Timestamp, before, after);
    }

    [Fact]
    public async Task OnActionExecutionAsync_WithValidModelState_CallsNext()
    {
        var httpContext = new DefaultHttpContext();

        var context = new ActionExecutingContext(
            new ActionContext(httpContext, new RouteData(), new ActionDescriptor()),
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            new object());

        var nextCalled = false;

        await new ValidationFilter().OnActionExecutionAsync(context, () =>
        {
            nextCalled = true;
            return Task.FromResult(new ActionExecutedContext(
                new ActionContext(httpContext, new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>(),
                new object()));
        });

        Assert.True(nextCalled);
        Assert.Null(context.Result);
    }
}
