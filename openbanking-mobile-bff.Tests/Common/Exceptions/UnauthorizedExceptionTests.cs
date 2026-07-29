using System.Net;
using openbanking_mobile_bff.Common.Exceptions;

namespace openbanking_mobile_bff.Tests.Common.Exceptions;

public sealed class UnauthorizedExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_SetsStatusCodeToUnauthorized()
    {
        var exception = new UnauthorizedException("Access denied.");
        
        Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
    }

    [Fact]
    public void Constructor_Always_SetsUnauthorizedErrorCode()
    {
        var exception = new UnauthorizedException("Access denied.");
        
        Assert.Equal("TR.OHVPS.Connection.Unauthorized", exception.ErrorCode);
    }

    [Fact]
    public void Constructor_WithMessage_SetsErrorMessageAndExceptionMessage()
    {
        var exception = new UnauthorizedException("Token has expired.");

        Assert.Equal("Token has expired.", exception.ErrorMessage);
        
        Assert.Equal("Token has expired.", exception.Message);
    }

    [Fact]
    public void Instance_IsAssignableToOhvpsExceptionAndException()
    {
        var exception = new UnauthorizedException("Access denied.");

        Assert.IsAssignableFrom<OhvpsException>(exception);
        Assert.IsAssignableFrom<Exception>(exception);
    }

    [Theory]
    [InlineData("Access denied.")]
    [InlineData("Token is invalid.")]
    [InlineData("Authorization header is missing.")]
    public void Constructor_WithVariousMessages_MapsMessageToMatchingProperties(string message)
    {
        var exception = new UnauthorizedException(message);

        Assert.Equal(message, exception.ErrorMessage);
        Assert.Equal(message, exception.Message);
        
        Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
        Assert.Equal("TR.OHVPS.Connection.Unauthorized", exception.ErrorCode);
    }
}
