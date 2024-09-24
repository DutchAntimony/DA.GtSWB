namespace DA.GtSWB.Common.Errors;

public abstract record NotFoundError : Error
{
    public abstract Type EntityType { get; }
}
