namespace DA.GtSWB.Common.Data;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> All { get; }
    void Add(TEntity entity);
    void Remove(TEntity entity);

    Task<List<TEntity>> QueryAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    Task<Result<TEntity>> SingleOrFailureAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
}
