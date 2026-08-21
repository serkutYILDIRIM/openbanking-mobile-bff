using openbanking_mobile_bff.Configuration;

namespace openbanking_mobile_bff.Tests.Configuration;

public sealed class RateLimitOptionsTests
{
    [Fact]
    public void Constructor_Always_InitializesDefaultValues()
    {
        var options = new RateLimitOptions();
        
        Assert.Equal(0, options.PermitLimit);
        Assert.Equal(0, options.WindowSeconds);
        Assert.Equal(0, options.QueueLimit);
    }

    [Fact]
    public void Properties_WithProvidedValues_PreservesAssignedState()
    {
        var options = new RateLimitOptions
        {
            PermitLimit = 120,
            WindowSeconds = 30,
            QueueLimit = 10
        };

        Assert.Equal(120, options.PermitLimit);
        Assert.Equal(30, options.WindowSeconds);
        Assert.Equal(10, options.QueueLimit);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(-1, -1, -1)]
    [InlineData(int.MaxValue, int.MaxValue, int.MaxValue)]
    public void Properties_WithEdgeValues_PreservesAssignedState(int permitLimit, int windowSeconds, int queueLimit)
    {
        var options = new RateLimitOptions
        {
            PermitLimit = permitLimit,
            WindowSeconds = windowSeconds,
            QueueLimit = queueLimit
        };

        Assert.Equal(permitLimit, options.PermitLimit);
        Assert.Equal(windowSeconds, options.WindowSeconds);
        Assert.Equal(queueLimit, options.QueueLimit);
    }
}
