using openbanking_mobile_bff.Configuration;

namespace openbanking_mobile_bff.Tests.Configuration;

public sealed class MicroserviceEndpointOptionsTests
{
    [Fact]
    public void Constructor_Always_InitializesDefaultValues()
    {
        var options = new MicroserviceEndpointOptions();

        Assert.Equal(string.Empty, options.YosMicroserviceBaseUrl);
        Assert.Equal(string.Empty, options.HhsMicroserviceBaseUrl);
        Assert.Equal(string.Empty, options.ApiGatewayBaseUrl);
    }

    [Fact]
    public void Properties_WithProvidedValues_PreservesAssignedState()
    {
        var options = new MicroserviceEndpointOptions
        {
            YosMicroserviceBaseUrl = "https://yos.local/api",
            HhsMicroserviceBaseUrl = "https://hhs.local/api",
            ApiGatewayBaseUrl = "https://gateway.local"
        };

        Assert.Equal("https://yos.local/api", options.YosMicroserviceBaseUrl);
        Assert.Equal("https://hhs.local/api", options.HhsMicroserviceBaseUrl);
        Assert.Equal("https://gateway.local", options.ApiGatewayBaseUrl);
    }

    [Theory]
    [InlineData("", "", "")]
    [InlineData("   ", "\t", "\r\n")]
    [InlineData("https://yos.local/v1?x=1", "https://hhs.local/v2#section", "https://gateway.local/path/")]
    public void Properties_WithEdgeValues_PreservesAssignedState(string yosMicroserviceBaseUrl, string hhsMicroserviceBaseUrl, string apiGatewayBaseUrl)
    {
        var options = new MicroserviceEndpointOptions
        {
            YosMicroserviceBaseUrl = yosMicroserviceBaseUrl,
            HhsMicroserviceBaseUrl = hhsMicroserviceBaseUrl,
            ApiGatewayBaseUrl = apiGatewayBaseUrl
        };

        Assert.Equal(yosMicroserviceBaseUrl, options.YosMicroserviceBaseUrl);
        Assert.Equal(hhsMicroserviceBaseUrl, options.HhsMicroserviceBaseUrl);
        Assert.Equal(apiGatewayBaseUrl, options.ApiGatewayBaseUrl);
    }
}