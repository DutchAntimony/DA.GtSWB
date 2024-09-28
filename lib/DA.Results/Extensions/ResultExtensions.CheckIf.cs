using System.Net.Http.Headers;

namespace DA.Results.Extensions;

public static partial class ResultExtensions
{
    public static Result CheckIf(this Result result, bool predicate, Result checkFunc)
    {
        if (!result.IsSuccess)
            return result;
        if (predicate == false)
            return result;
        return result.Bind(checkFunc); // Bind and check in the context of a result without value are the same.
    }

    public static Result CheckIf(this Result result, Func<bool> predicate, Func<Result> checkFunc)
    {
        if (!result.IsSuccess)
            return result;
        if (predicate() == false)
            return result;
        return result.Bind(checkFunc()); // Bind and check in the context of a result without value are the same.
    }

    public static Result<TValue> CheckIf<TValue>(this Result<TValue> result, bool predicate, Result checkFunc)
    {
        if (!result.IsSuccess)
            return result;
        if (predicate == false)
            return result;
        return result.Check(checkFunc);
    }

    public static Result<TValue> CheckIf<TValue>(this Result<TValue> result, Func<TValue, bool> predicate, Result check)
    {
        if (!result.TryGetValue(out var value))
            return result;
        if (predicate(value) == false)
            return result;
        return result.Check(check);
    }

    public static Result<TValue> CheckIf<TValue>(this Result<TValue> result, Func<TValue, bool> predicate, Func<TValue, Result> checkFunc)
    {
        if (!result.TryGetValue(out var value))
            return result;
        if (predicate(value) == false)
            return result;
        return result.Check(checkFunc);
    }

    public static async Task<Result> CheckIfAsync(this Result result, Func<bool> predicate, Task<Result> checkTask)
    {
        if (!result.IsSuccess)
            return result;
        if (predicate() == false)
            return result;
        return await result.BindAsync(checkTask); // Bind and check in the context of a result without value are the same.
    }

    public static async Task<Result<TValue>> CheckIfAsync<TValue>(this Result<TValue> result, Func<TValue, bool> predicate, Task<Result> checkTask)
    {
        if (!result.TryGetValue(out var value))
            return result;
        if (predicate(value) == false)
            return result;
        return await result.CheckAsync(checkTask);
    }

    public static async Task<Result<TValue>> CheckIfAsync<TValue>(this Result<TValue> result, Func<TValue, bool> predicate, Func<TValue, Task<Result>> checkTaskFunc)
    {
        if (!result.TryGetValue(out var value))
            return result;
        if (predicate(value) == false)
            return result;
        return await result.CheckAsync(checkTaskFunc(value));
    }

    public static async Task<Result> CheckIf(this Task<Result> resultTask, bool predicate, Result check)
    {
        var result = await resultTask;
        return result.CheckIf(predicate, check);
    }

    public static async Task<Result> CheckIf(this Task<Result> resultTask, Func<bool> predicate, Func<Result> checkFunc)
    {
        var result = await resultTask;
        return result.CheckIf(predicate, checkFunc);
    }

    public static async Task<Result<TValue>> CheckIf<TValue>(this Task<Result<TValue>> resultTask, bool predicate, Result check)
    {
        var result = await resultTask;
        return result.CheckIf(predicate, check);
    }

    public static async Task<Result<TValue>> CheckIf<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, bool> predicate, Result check)
    {
        var result = await resultTask;
        return result.CheckIf(predicate, check);
    }

    public static async Task<Result<TValue>> CheckIf<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, bool> predicate, Func<TValue, Result> checkFunc)
    {
        var result = await resultTask;
        return result.CheckIf(predicate, checkFunc);
    }

    public static async Task<Result> CheckIfAsync(this Task<Result> resultTask, Func<bool> predicate, Task<Result> checkTask)
    {
        var result = await resultTask;
        return await result.CheckIfAsync(predicate, checkTask);
    }

    public static async Task<Result<TValue>> CheckIfAsync<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, bool> predicate, Task<Result> checkTask)
    {
        var result = await resultTask;
        return await result.CheckIfAsync(predicate, checkTask);
    }

    public static async Task<Result<TValue>> CheckIfAsync<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, bool> predicate, Func<TValue, Task<Result>> checkTaskFunc)
    {
        var result = await resultTask;
        return await result.CheckIfAsync(predicate, checkTaskFunc);
    }
}