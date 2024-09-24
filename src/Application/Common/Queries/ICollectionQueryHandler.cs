using MediatR;

namespace DA.GtSWB.Application.Common.Queries;

internal interface ICollectionQueryHandler<TCollectionQuery, TResponse>
    : IRequestHandler<TCollectionQuery, Result<IQueryable<TResponse>>>
    where TCollectionQuery : ICollectionQuery<TResponse>
{ }