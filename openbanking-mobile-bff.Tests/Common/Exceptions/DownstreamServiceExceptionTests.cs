using System.Net;
using openbanking_mobile_bff.Common.Exceptions;

namespace openbanking_mobile_bff.Tests.Common.Exceptions;

public sealed class DownstreamServiceExceptionTests
{
    [Fact]
    public void Constructor_WithServiceNameMessageAndStatusCode_SetsServiceName()
    {
        var exception = new DownstreamServiceException("hhs-service", "Downstream call failed", HttpStatusCode.BadGateway);

        Assert.Equal("hhs-service", exception.ServiceName);
    }

    [Fact]
    public void Constructor_WithStatusCode_PropagatesStatusCodeToBase()
    {
        var exception = new DownstreamServiceException("yos-service", "Downstream call failed", HttpStatusCode.ServiceUnavailable);
        Assert.Equal(HttpStatusCode.ServiceUnavailable, exception.StatusCode);
    }

    [Fact]
    public void Constructor_Always_SetsDownstreamErrorCode()
    {
        var exception = new DownstreamServiceException("hhs-service", "Downstream call failed", HttpStatusCode.BadGateway);

        Assert.Equal("TR.OHVPS.Connection.DownstreamError", exception.ErrorCode);
    }

    [Fact]
    public void Constructor_WithMessage_SetsErrorMessageAndExceptionMessage()
    {
        var exception = new DownstreamServiceException("hhs-service", "Gateway timeout occurred", HttpStatusCode.GatewayTimeout);

        Assert.Equal("Gateway timeout occurred", exception.ErrorMessage);
        Assert.Equal("Gateway timeout occurred", exception.Message);
    }

    [Fact]
    public void Instance_IsAssignableToOhvpsExceptionAndException()
    {
        var exception = new DownstreamServiceException("hhs-service", "Downstream call failed", HttpStatusCode.BadGateway);

        Assert.IsAssignableFrom<OhvpsException>(exception);
        Assert.IsAssignableFrom<Exception>(exception);
    }

    [Theory]
    [InlineData("hhs-service", "Not found downstream", HttpStatusCode.NotFound)]
    [InlineData("yos-service", "Unauthorized downstream", HttpStatusCode.Unauthorized)]
    [InlineData("api-gateway", "Internal downstream error", HttpStatusCode.InternalServerError)]
    public void Constructor_WithVariousInputs_MapsEachValueToMatchingProperty(
        string serviceName,
        string message,
        HttpStatusCode statusCode)
    {
        var exception = new DownstreamServiceException(serviceName, message, statusCode);

        Assert.Equal(serviceName, exception.ServiceName);
        Assert.Equal(message, exception.ErrorMessage);
        Assert.Equal(statusCode, exception.StatusCode);
        Assert.Equal("TR.OHVPS.Connection.DownstreamError", exception.ErrorCode);
    }
}
