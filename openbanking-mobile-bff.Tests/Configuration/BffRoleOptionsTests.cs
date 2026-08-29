using openbanking_mobile_bff.Configuration;

namespace openbanking_mobile_bff.Tests.Configuration;

public sealed class BffRoleOptionsTests
{
    [Fact]
    public void Constructor_Always_InitializesDefaultValues()
    {
        var options = new BffRoleOptions();

        Assert.Equal("YOS", options.Role);
    }

    [Fact]
    public void Properties_WithProvidedValues_PreservesAssignedState()
    {
        var options = new BffRoleOptions
        {
            Role = "HHS"
        };

        Assert.Equal("HHS", options.Role);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("MOBILE_BFF")]
    public void Properties_WithEdgeValues_PreservesAssignedState(string role)
    {
        var options = new BffRoleOptions
        {
            Role = role
        };

        Assert.Equal(role, options.Role);
    }
}