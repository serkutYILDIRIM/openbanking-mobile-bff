namespace openbanking_mobile_bff.Infrastructure.HttpClients;

public interface IYosMicroserviceClient
{
    HttpClient Client { get; }
}

public interface IHhsMicroserviceClient
{
    HttpClient Client { get; }
}

public interface IApiGatewayClient
{
    HttpClient Client { get; }
}

public sealed class YosMicroserviceClient : IYosMicroserviceClient
{
    public HttpClient Client { get; }

    public YosMicroserviceClient(HttpClient client)
    {
        Client = client;
    }
}

public sealed class HhsMicroserviceClient : IHhsMicroserviceClient
{
    public HttpClient Client { get; }

    public HhsMicroserviceClient(HttpClient client)
    {
        Client = client;
    }
}

public sealed class ApiGatewayClient : IApiGatewayClient
{
    public HttpClient Client { get; }

    public ApiGatewayClient(HttpClient client)
    {
        Client = client;
    }
}

