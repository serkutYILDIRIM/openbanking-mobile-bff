namespace openbanking_mobile_bff.Configuration;

public sealed class MicroserviceEndpointOptions
{
    public string YosMicroserviceBaseUrl { get; set; } = string.Empty;
    public string HhsMicroserviceBaseUrl { get; set; } = string.Empty;
    public string ApiGatewayBaseUrl { get; set; } = string.Empty;
}

