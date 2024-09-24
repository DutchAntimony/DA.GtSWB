using MediatR;

namespace DA.GtSWB.Application.Common.Queries;

internal interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
