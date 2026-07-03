using openbanking_mobile_bff.Common.Utilities;

namespace openbanking_mobile_bff.Tests.Common.Utilities;

public sealed class DateUtilTests
{
    [Theory]
    [InlineData(2024, 1, 15, 10, 30, 45, 123, "2024-01-15T10:30:45.123Z")]
    [InlineData(2024, 1, 15, 10, 30, 45, 0, "2024-01-15T10:30:45.000Z")]
    [InlineData(2024, 3, 5, 4, 7, 9, 8, "2024-03-05T04:07:09.008Z")]
    [InlineData(1999, 12, 31, 23, 59, 59, 999, "1999-12-31T23:59:59.999Z")]
    public void ToIso8601_WithUtcDateTime_ReturnsZeroPaddedIso8601String(
        int year,
        int month,
        int day,
        int hour,
        int minute,
        int second,
        int millisecond,
        string expected)
    {
        var dateTime = new DateTime(year, month, day, hour, minute, second, millisecond, DateTimeKind.Utc);
        var result = DateUtil.ToIso8601(dateTime);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToIso8601_WithSubMillisecondTicks_TruncatesToMilliseconds()
    {
        var dateTime = new DateTime(2024, 1, 15, 10, 30, 45, 123, DateTimeKind.Utc).AddTicks(9000);
        var result = DateUtil.ToIso8601(dateTime);
        
        Assert.Equal("2024-01-15T10:30:45.123Z", result);
    }

    [Fact]
    public void ToIso8601_WithNonUtcKind_AppendsLiteralZWithoutTimeZoneConversion()
    {
        var dateTime = new DateTime(2024, 1, 15, 10, 30, 45, 123, DateTimeKind.Local);
        var result = DateUtil.ToIso8601(dateTime);

        Assert.Equal("2024-01-15T10:30:45.123Z", result);
    }

    [Fact]
    public void ParseIso8601_WithValidUtcString_ReturnsExpectedComponentsAndUtcKind()
    {
        var result = DateUtil.ParseIso8601("2024-01-15T10:30:45.123Z");

        Assert.Equal(2024, result.Year);
        Assert.Equal(1, result.Month);
        Assert.Equal(15, result.Day);
        Assert.Equal(10, result.Hour);
        Assert.Equal(30, result.Minute);
        Assert.Equal(45, result.Second);
        Assert.Equal(123, result.Millisecond);
        Assert.Equal(DateTimeKind.Utc, result.Kind);
    }

    [Fact]
    public void ParseIso8601_WithInvalidString_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => DateUtil.ParseIso8601("not-a-valid-date"));
    }

    [Fact]
    public void ParseIso8601_WithNullValue_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => DateUtil.ParseIso8601(null!));
    }

    [Fact]
    public void ToIso8601_ThenParseIso8601_RoundTripsUtcValue()
    {
        var value = new DateTime(2024, 1, 15, 10, 30, 45, 123, DateTimeKind.Utc);
        var result = DateUtil.ParseIso8601(DateUtil.ToIso8601(value));

        Assert.Equal(value, result);
        Assert.Equal(DateTimeKind.Utc, result.Kind);
    }
}

