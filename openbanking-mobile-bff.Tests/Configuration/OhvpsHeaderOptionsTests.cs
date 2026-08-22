using openbanking_mobile_bff.Configuration;

namespace openbanking_mobile_bff.Tests.Configuration;

public sealed class OhvpsHeaderOptionsTests
{
    [Fact]
    public void Constructor_Always_InitializesDefaultValues()
    {
        var options = new OhvpsHeaderOptions();
        Assert.Equal("X-Request-ID", options.RequestIdHeader);
        Assert.Equal("X-ASPSP-Code", options.AspspCodeHeader);
        Assert.Equal("X-TPP-Code", options.TppCodeHeader);
        Assert.Equal("X-JWS-Signature", options.JwsSignatureHeader);
    }

    [Fact]
    public void Properties_WithProvidedValues_PreservesAssignedState()
    {
        var options = new OhvpsHeaderOptions
        {
            RequestIdHeader = "X-Custom-Request-ID",
            AspspCodeHeader = "X-Custom-ASPSP-Code",
            TppCodeHeader = "X-Custom-TPP-Code",
            JwsSignatureHeader = "X-Custom-JWS-Signature"
        };

        Assert.Equal("X-Custom-Request-ID", options.RequestIdHeader);
        Assert.Equal("X-Custom-ASPSP-Code", options.AspspCodeHeader);
        Assert.Equal("X-Custom-TPP-Code", options.TppCodeHeader);
        Assert.Equal("X-Custom-JWS-Signature", options.JwsSignatureHeader);
    }

    [Theory]
    [InlineData("", "", "", "")]
    [InlineData("   ", "\t", "\r\n", " ")]
    [InlineData("Header-1", "Header-2", "Header-3", "Header-4")]
    public void Properties_WithEdgeValues_PreservesAssignedState(string requestIdHeader, string aspspCodeHeader, string tppCodeHeader, string jwsSignatureHeader)
    {
        var options = new OhvpsHeaderOptions
        {
            RequestIdHeader = requestIdHeader,
            AspspCodeHeader = aspspCodeHeader,
            TppCodeHeader = tppCodeHeader,
            JwsSignatureHeader = jwsSignatureHeader
        };

        Assert.Equal(requestIdHeader, options.RequestIdHeader);
        Assert.Equal(aspspCodeHeader, options.AspspCodeHeader);
        Assert.Equal(tppCodeHeader, options.TppCodeHeader);
        Assert.Equal(jwsSignatureHeader, options.JwsSignatureHeader);
    }
}

