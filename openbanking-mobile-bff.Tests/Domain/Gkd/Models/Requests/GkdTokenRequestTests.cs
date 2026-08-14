using System.Text.Json;
using openbanking_mobile_bff.Domain.Gkd.Models.Requests;

namespace openbanking_mobile_bff.Tests.Domain.Gkd.Models.Requests;

public sealed class GkdTokenRequestTests
{
    [Fact]
    public void Constructor_Always_InitializesWithNullValues()
    {
        var request = new GkdTokenRequest();

        Assert.Null(request.AuthorizationCode);
    }

    [Fact]
    public void Properties_WithProvidedValues_PreservesAssignedState()
    {
        var request = new GkdTokenRequest
        {
            AuthorizationCode = "authorization-code-123"
        };

        Assert.Equal("authorization-code-123", request.AuthorizationCode);
    }

    [Fact]
    public void Serialize_WithAuthorizationCode_UsesExpectedContractFieldName()
    {
        var request = new GkdTokenRequest
        {
            AuthorizationCode = "authorization-code-123"
        };

        var json = JsonSerializer.Serialize(request);
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.True(root.TryGetProperty("bltmKodu", out var authorizationCode));
        Assert.Equal("authorization-code-123", authorizationCode.GetString());
    }
}