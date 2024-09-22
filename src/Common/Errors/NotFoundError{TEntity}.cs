using DA.GtSWB.Common.Data;

namespace DA.GtSWB.Common.Errors;

public record NotFoundError<TEntity>(ISpecification<TEntity> Specification) : NotFoundError
    where TEntity : class
{
    public override Type EntityType => typeof(TEntity);
    public override string GetMessage() => $"Could not find entity of type {typeof(TEntity).Name} using specification\n{Specification}.";
}