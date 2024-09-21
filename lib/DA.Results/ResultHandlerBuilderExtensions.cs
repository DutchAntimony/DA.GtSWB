using DA.Results.ResultHandlers;

namespace DA.Results;

public static class ResultHandlerBuilderExtensions
{
    public static ResultHandlerBuilder Act(this Result result) => new(result);
    public static ResultHandlerBuilder<TValue> Act<TValue>(this Result<TValue> result) => new(result);

    public static TValue ReduceOrThrow<TValue>(this Result<TValue> result, Exception? ex = default)
    {
        if (!result.TryGetValue(out var value))
        {
            throw ex ?? throw new Exception();
        }
        return value;
    }

    public static void ExampleSyntax(Result<int> result)
    {
        result.Act()
            .OnSuccess(value => Console.WriteLine($"Success {value}"))
            .OnDefaultWarning(warning => Console.WriteLine($"Warning: {warning.GetMessage()}"))
            .OnDefaultError(error => Console.WriteLine($"Unmatched error {error.GetType().Name}: {error.GetMessage()}"))
            .Execute();

        result.Act()
            .OnSuccess(Console.WriteLine)
            .OnDefaultWarning(warning => Console.WriteLine($"Warning: {warning.GetMessage()}"))
            .OnDefaultError(error => Console.WriteLine(error.GetMessage()))
            .Execute();
    }
}
