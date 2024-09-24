namespace DA.Results.Extensions;

/// <summary>
/// Check:
/// Checks a predicate(which returns a result for the messages) on the T of the Result { T }. Result is the same T
/// </summary>
public static partial class ResultExtensions
{
    public static Result Check(this Result result, Result check)
    {
        return result.Bind(check);
    }

    public static Result Check(this Result result, Func<Result> checkFunc)
    {
        return result.Bind(checkFunc);
    }

    public static Result<T> Check<T>(this Result<T> result, Func<T, Result> checkFunc)
    {
        if (!result.TryGetValue(out var value))
            return result;
        var check = checkFunc(value);
        return result.Check(check);
    }

    public static async Task<Result<T>> CheckAsync<T, TResult>(this Result<T> result, Task<TResult> checkTask)
        where TResult : Result
    {
        if (!result.IsSuccess)
            return result;
        var check = await checkTask;
        return result.Check(check);
    }

    public static async Task<Result<T>> CheckAsync<T>(this Result<T> result, Func<T, Task<Result>> checkTaskFunc)
    {
        if (!result.TryGetValue(out var value))
            return result;
        var check = await checkTaskFunc(value);
        return result.Check(check);
    }

    public static async Task<Result<T>> Check<T>(this Task<Result<T>> resultTask, Result check)
    {
        var result = await resultTask;
        return result.Check(check);
    }

    public static async Task<Result<T>> Check<T>(this Task<Result<T>> resultTask, Func<T, Result> checkFunc)
    {
        var result = await resultTask;
        return Check(result, checkFunc);
    }

    public static async Task<Result<T>> CheckAsync<T, TResult>(this Task<Result<T>> resultTask, Task<TResult> checkTask)
        where TResult : Result
    {
        var result = await resultTask;
        return await CheckAsync(result, checkTask);
    }

    public static async Task<Result<T>> CheckAsync<T>(this Task<Result<T>> resultTask, Func<T, Task<Result>> checkTaskFunc)
    {
        var result = await resultTask;
        return await CheckAsync(result, checkTaskFunc);
    }

    public static async Task<Result<(T1, T2)>> Check<T1, T2>(this Task<Result<(T1, T2)>> resultTask, Func<T1, T2, Result> checkFunc)
    {
        var result = await resultTask;
        if (!result.TryGetValue(out var value))
            return result;
        var check = checkFunc(value.Item1, value.Item2);
        return result.Check(check);
    }
}