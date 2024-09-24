using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Numerics;

namespace DA.GtSWB.Persistence.Extensions;

public static class IQueryableExtensions
{
    public static async Task<TNumber> MaxOrDefaultAsync<TEntity, TNumber>(this IQueryable<TEntity> queryable, Expression<Func<TEntity, TNumber>> selector, TNumber fallback, CancellationToken cancellationToken = default)
        where TNumber : INumber<TNumber>
    {
        return await queryable.AnyAsync(cancellationToken)
            ? await queryable.MaxAsync(selector, cancellationToken)
            : fallback;
    }

    public static async Task<TNumber?> MaxOrNullableAsync<TEntity, TNumber>(this IQueryable<TEntity> queryable, Expression<Func<TEntity, TNumber>> selector, CancellationToken cancellationToken = default)
                where TNumber : INumber<TNumber>
    {
        return await queryable.AnyAsync(cancellationToken)
            ? await queryable.MaxAsync(selector, cancellationToken)
            : default;
    }
}
