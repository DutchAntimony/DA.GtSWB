using DA.GtSWB.Common.Data;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace DA.GtSWB.Persistence.Common;

public class QueryableSpecification<TEntity> : ISpecification<TEntity> where TEntity : class
{
    public IEnumerable<Expression<Func<TEntity, bool>>> Conditions => _conditions;
    public IEnumerable<(Expression<Func<TEntity, object>> keySelector, bool isAscending)> OrderByExpressions => _orderByExpressions;

    private readonly ImmutableList<Expression<Func<TEntity, bool>>> _conditions;
    private readonly ImmutableList<(Expression<Func<TEntity, object>> keySelector, bool isAscending)> _orderByExpressions;

    public QueryableSpecification() : this([], []) { }

    private QueryableSpecification(
        ImmutableList<Expression<Func<TEntity, bool>>> conditions,
        ImmutableList<(Expression<Func<TEntity, object>>, bool)> orderByExpressions) =>
        (_conditions, _orderByExpressions) = (conditions, orderByExpressions);

    public static QueryableSpecification<TEntity> Empty =>
        new([], []);

    public ISpecification<TEntity> And(Expression<Func<TEntity, bool>> condition) =>
        new QueryableSpecification<TEntity>(_conditions.Add(condition), _orderByExpressions);

    public ISpecification<TEntity> OrderBy(Expression<Func<TEntity, object>> keySelector) =>
        new QueryableSpecification<TEntity>(_conditions, _orderByExpressions.Add((keySelector, true)));

    public ISpecification<TEntity> OrderByDescending(Expression<Func<TEntity, object>> keySelector) =>
        new QueryableSpecification<TEntity>(_conditions, _orderByExpressions.Add((keySelector, false)));
}