using MediatR;

namespace DA.GtSWB.Application.Common.Queries;

internal interface ICollectionQuery<TResponse> : IRequest<Result<IQueryable<TResponse>>> { }
