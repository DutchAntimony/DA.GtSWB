using DA.GtSWB.Application.Common.Queries;
using DA.GtSWB.Domain.ServiceDefinitions;
using MediatR;

namespace DA.GtSWB.Application.Behaviour;
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
        if (request is IQuery<IResult> or ICollectionQuery<IResult>)
        {
            return await next();
        }

        using var transaction = unitOfWork.BeginTransaction();
        try
        {
            var response = await next();
            if (response.IsSuccess)
            {
                await unitOfWork.CommitAsync(cancellationToken);
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
            }

            return response;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
