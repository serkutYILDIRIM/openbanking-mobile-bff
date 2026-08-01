using System.Net;
using openbanking_mobile_bff.Common.Exceptions;

namespace openbanking_mobile_bff.Tests.Common.Exceptions;

public sealed class OhvpsExceptionTests
{
    [Fact]
    public void Constructor_WithProvidedValues_SetsStatusCodeErrorCodeAndMessageProperties()
    {
        var exception = new OhvpsException(
            HttpStatusCode.BadRequest,
            "TR.OHVPS.Invalid.Request",
            "The request payload is invalid.");

        Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
        Assert.Equal("TR.OHVPS.Invalid.Request", exception.ErrorCode);
        Assert.Equal("The request payload is invalid.", exception.ErrorMessage);
        Assert.Equal("The request payload is invalid.", exception.Message);
    }
}