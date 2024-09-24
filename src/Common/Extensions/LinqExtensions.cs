namespace DA.GtSWB.Common.Extensions;
public static class LinqExtensions
{
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        source.EnsureNotNull("Linq.Foreach can not be used on an null IEnumerable<T>");
        action.EnsureNotNull("Linq.Foreach can not be used with a null action.");

        foreach (var item in source)
        { action(item); }
    }
}
