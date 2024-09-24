using DA.GtSWB.Application.Common.Queries;
using DA.GtSWB.Domain.ServiceDefinitions;
using MediatR;

namespace DA.ApplicationLibrary.Behaviours;
public sealed class UnitOfWorkPipelineBehaviour<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IResult
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();
        if (response.IsSuccess && request is not IQuery<IResult> or ICollectionQuery<IResult>)
        {
            await unitOfWork.CommitAsync(cancellationToken);
        }

        return response;
    }
}
