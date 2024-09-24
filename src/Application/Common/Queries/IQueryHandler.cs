using MediatR;

namespace DA.GtSWB.Application.Common.Queries;

internal interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{ }
