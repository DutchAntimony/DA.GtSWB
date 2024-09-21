namespace DA.GtSWB.Common.Extensions;
public static class StringExtensions
{
    public static string Sanitize(this string value)
    {
        value = value.Trim().Normalize();

        return string.IsNullOrWhiteSpace(value) ? string.Empty
            : string.Concat(value[0].ToString().ToUpper(), value.AsSpan(1));
    }
}
