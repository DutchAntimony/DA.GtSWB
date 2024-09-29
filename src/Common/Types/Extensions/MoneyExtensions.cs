namespace DA.GtSWB.Common.Types.Extensions;

public static class MoneyExtensions
{
    public static Money Sum(this IEnumerable<Money> source)
    {
        return source.Aggregate(Money.Zero, (acc, money) => acc + money);
    }

    public static Money Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Money> selector)
    {
        return source.Select(selector).Aggregate(Money.Zero, (acc, money) => acc + money);
    }
}