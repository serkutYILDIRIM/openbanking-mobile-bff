using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using openbanking_mobile_bff.Extensions;
using openbanking_mobile_bff.Infrastructure.HttpClients.ApiGateway;
using openbanking_mobile_bff.Infrastructure.HttpClients.Hhs;
using openbanking_mobile_bff.Infrastructure.HttpClients.Yos;

namespace openbanking_mobile_bff.Tests.Extensions;

public sealed class HttpClientExtensionsTests
{
    [Fact]
    public void AddBffHttpClients_WithConfiguredEndpoints_ConfiguresBaseAddressesForAllClients()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["MicroserviceEndpoints:YosMicroserviceBaseUrl"] = "https://yos.example.com",
                ["MicroserviceEndpoints:HhsMicroserviceBaseUrl"] = "https://hhs.example.com",
                ["MicroserviceEndpoints:ApiGatewayBaseUrl"] = "https://gateway.example.com"
            })
            .Build();

        services.AddBffHttpClients(configuration);

        using var provider = services.BuildServiceProvider();

        var yosClient = provider.GetRequiredService<IYosMicroserviceClient>();
        var hhsClient = provider.GetRequiredService<IHhsMicroserviceClient>();
        var apiGatewayClient = provider.GetRequiredService<IApiGatewayClient>();

        Assert.Equal(new Uri("https://yos.example.com"), GetBaseAddress(yosClient));
        Assert.Equal(new Uri("https://hhs.example.com"), GetBaseAddress(hhsClient));
        Assert.Equal(new Uri("https://gateway.example.com"), GetBaseAddress(apiGatewayClient));
    }

    private static Uri GetBaseAddress(object client)
    {
        var httpClientField = client.GetType().GetField("_httpClient", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(httpClientField);

        var httpClient = httpClientField!.GetValue(client) as HttpClient;
        Assert.NotNull(httpClient);

        return httpClient!.BaseAddress!;
    }
}
