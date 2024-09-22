namespace DA.GtSWB.Domain.Extensions;

public static class OptionExtensions
{
    public static T? ToNullIf<T>(this Option<T> value, Predicate<T> predicate)
        where T : class
    {
        return value.Match(val => predicate(val) ? null : val, () => null);
    }

    public static T? ToNullStructIf<T>(this Option<T> value, Predicate<T> predicate)
        where T : struct
    {
        return value.Match<T, T?>(val => predicate(val) ? null : val, () => null);
    }
}

public static class ResultExtensions
{
    public static Result<TOut> Cast<TIn, TOut>(this Result<TIn> result)
        where TIn : TOut
    {
        return result.Map(t => (TOut)t);
    }
}
