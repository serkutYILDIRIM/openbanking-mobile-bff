using openbanking_mobile_bff.Configuration;

namespace openbanking_mobile_bff.Tests.Configuration;

public sealed class JwtValidationOptionsTests
{
    [Fact]
    public void Constructor_Always_InitializesDefaultValues()
    {
        var options = new JwtValidationOptions();
        Assert.Equal(string.Empty, options.Issuer);
        Assert.Equal(string.Empty, options.Audience);
        Assert.Equal(string.Empty, options.JwksUri);
        Assert.False(options.ValidateLifetime);
    }

    [Fact]
    public void Properties_WithProvidedValues_PreservesAssignedState()
    {
        var options = new JwtValidationOptions
        {
            Issuer = "https://issuer.example.com",
            Audience = "mobile-bff-client",
            JwksUri = "https://issuer.example.com/.well-known/jwks.json",
            ValidateLifetime = true
        };

        Assert.Equal("https://issuer.example.com", options.Issuer);
        Assert.Equal("mobile-bff-client", options.Audience);
        Assert.Equal("https://issuer.example.com/.well-known/jwks.json", options.JwksUri);
        Assert.True(options.ValidateLifetime);
    }

    [Theory]
    [InlineData("", "", "", false)]
    [InlineData("   ", "\t", "\r\n", true)]
    [InlineData("issuer-1", "audience-1", "jwks-1", false)]
    public void Properties_WithEdgeValues_PreservesAssignedState(string issuer, string audience, string jwksUri, bool validateLifetime)
    {
        var options = new JwtValidationOptions
        {
            Issuer = issuer,
            Audience = audience,
            JwksUri = jwksUri,
            ValidateLifetime = validateLifetime
        };

        Assert.Equal(issuer, options.Issuer);
        Assert.Equal(audience, options.Audience);
        Assert.Equal(jwksUri, options.JwksUri);
        Assert.Equal(validateLifetime, options.ValidateLifetime);
    }
}
