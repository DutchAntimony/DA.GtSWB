namespace DA.GtSWB.Common.Extensions;
public static class DateTimeExtensions
{
    public static string ToFileNameFormat(this DateTime date) => date.ToString("yyyy-MM-dd_HH-mm-ss");

}
