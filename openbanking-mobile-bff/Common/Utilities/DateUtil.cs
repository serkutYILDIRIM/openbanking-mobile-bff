namespace openbanking_mobile_bff.Common.Utilities;

public static class DateUtil
{
    public static string ToIso8601(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
    }

    public static DateTime ParseIso8601(string value)
    {
        return DateTime.Parse(value, null, System.Globalization.DateTimeStyles.RoundtripKind);
    }
}

