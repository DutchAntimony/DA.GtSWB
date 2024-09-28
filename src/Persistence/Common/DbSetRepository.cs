using DA.GtSWB.Common.Data;
using DA.GtSWB.Common.Errors;
using Microsoft.EntityFrameworkCore;

namespace DA.GtSWB.Persistence.Common;

public class DbSetRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet;

    public DbSetRepository(DbSet<TEntity> dbSet, IQueryable<TEntity> baseQuery) =>
        (_dbSet, All) = (dbSet, baseQuery);

    public IQueryable<TEntity> All { get; }

    public void Add(TEntity entity) => _dbSet.Add(entity);

    public void AddRange(IEnumerable<TEntity> entities) => 
        _dbSet.AddRange(entities);

    public void Remove(TEntity entity) => _dbSet.Remove(entity);

    public async Task<List<TEntity>> QueryAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default) =>
        await Apply(specification).ToListAsync(cancellationToken);

    public async Task<Result<TEntity>> SingleOrFailureAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        var entity = await Apply(specification).SingleOrDefaultAsync(cancellationToken);
        return entity is not null ? entity : new NotFoundError<TEntity>(specification);
    }

    private IQueryable<TEntity> Apply(ISpecification<TEntity> specification) =>
        specification is QueryableSpecification<TEntity> queryableSpecification ? Apply(queryableSpecification)
        : throw new ArgumentException("Specification is not of expected implementation type", nameof(specification));

    private IQueryable<TEntity> Apply(QueryableSpecification<TEntity> specification)
    {
        var filteredQuery = specification.Conditions
            .Aggregate(All, (query, condition) => query.Where(condition));

        using var orderByEnumerator =
            specification.OrderByExpressions.GetEnumerator();

        if (!orderByEnumerator.MoveNext())
        {
            return filteredQuery;
        }

        var orderedQuery = orderByEnumerator.Current.isAscending
            ? filteredQuery.OrderBy(orderByEnumerator.Current.keySelector)
            : filteredQuery.OrderByDescending(orderByEnumerator.Current.keySelector);

        while (orderByEnumerator.MoveNext())
        {
            orderedQuery = orderByEnumerator.Current.isAscending
                ? orderedQuery.ThenBy(orderByEnumerator.Current.keySelector)
                : orderedQuery.ThenByDescending(orderByEnumerator.Current.keySelector);
        }

        return orderedQuery;
    }
}
