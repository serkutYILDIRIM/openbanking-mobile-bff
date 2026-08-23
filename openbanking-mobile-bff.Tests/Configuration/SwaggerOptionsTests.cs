using openbanking_mobile_bff.Configuration;

namespace openbanking_mobile_bff.Tests.Configuration;

public sealed class SwaggerOptionsTests
{
    [Fact]
    public void Constructor_Always_InitializesDefaultValues()
    {
        var options = new SwaggerOptions();

        Assert.Equal(string.Empty, options.Title);
        Assert.Equal(string.Empty, options.Version);
        Assert.Equal(string.Empty, options.Description);
    }

    [Fact]
    public void Properties_WithProvidedValues_PreservesAssignedState()
    {
        var options = new SwaggerOptions
        {
            Title = "Open Banking Mobile BFF API",
            Version = "v1",
            Description = "Public API documentation"
        };

        Assert.Equal("Open Banking Mobile BFF API", options.Title);
        Assert.Equal("v1", options.Version);
        Assert.Equal("Public API documentation", options.Description);
    }

    [Theory]
    [InlineData("", "", "")]
    [InlineData("swagger", "2026.08", "desc")]
    [InlineData("x", "1", " ")]
    public void Properties_WithEdgeValues_PreservesAssignedState(string title, string version, string description)
    {
        var options = new SwaggerOptions
        {
            Title = title,
            Version = version,
            Description = description
        };

        Assert.Equal(title, options.Title);
        Assert.Equal(version, options.Version);
        Assert.Equal(description, options.Description);
    }
}

