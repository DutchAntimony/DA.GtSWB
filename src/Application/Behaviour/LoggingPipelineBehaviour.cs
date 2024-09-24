using DA.GtSWB.Common.Errors;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace DA.ApplicationLibrary.Behaviours;
public sealed class LoggingPipelineBehaviour<TRequest, TResponse>(ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).ReadableName();
        var stopwatch = Stopwatch.StartNew();

        logger.LogInformation("Starting pipeline for {request}", requestName);

        var result = await next();
        PipelineSpecificActions(result, requestName);

        stopwatch.Stop();
        var durationInMs = stopwatch.ElapsedMilliseconds;

        logger.LogInformation("Ending pipeline for {request}, elapsed time = {duration} ms.", requestName, durationInMs);
        if (durationInMs > 1000)
        {
            logger.LogWarning("Request {request} has taken {duration} ms. to complete which is more than the warning threshold of 1000 ms.", requestName, durationInMs);
        }

        return result;
    }

    public void PipelineSpecificActions(TResponse result, string request)
    {
        Action successAction = IsResultOfT(result, out var type)
            ? () => logger.LogInformation("{request} returned object of type {type}", request, type.ReadableName())
            : () => logger.LogInformation("{request} completed successfull.", request);

        result.Act()
            .OnSuccess(successAction)
            .OnDefaultWarning((warning) => logger.LogWarning("Command {request} resulted in a warning. Message: {warningMessage}", request, warning.GetMessage()))
            .OnError<ValidationError>((valError) => logger.LogWarning("Command {request} resulted in a validation error. Messages: {validationMessages}", request, valError.GetMessage()))
            .OnError<NotFoundError>((notFoundError) => logger.LogWarning("Command {request} could not find entity of type {type}", request, notFoundError.EntityType.Name))
            .OnDefaultError(error => logger.LogError("Command {request} resulted in an error. Message: {errorMessage}", request, error.GetMessage()))
            .Execute();
    }

    private static bool IsResultOfT(Result result, [NotNullWhen(true), MaybeNullWhen(false)] out Type? typeOfT)
    {
        var resultType = result.GetType();
        if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            typeOfT = resultType.GetGenericArguments()[0];
            return true;
        }
        else
        {
            typeOfT = null;
            return false;
        }
    }
}