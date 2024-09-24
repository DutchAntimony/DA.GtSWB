namespace DA.Results.Extensions;

/// <summary>
/// Map
/// Map a Result to a Result{T} using a value or function for T.
/// </summary>
public static partial class ResultExtensions
{
    #region Map from a Result to a Result{T} by providing a value.

    public static Result<T> Map<T>(this Result result, Func<T> func)
    {
        if (!result.IsSuccess)
            return new(result.Issue);
        return result.Map(func());
    }

    public static async Task<Result<T>> MapAsync<T>(this Result result, Task<T> task)
    {
        if (!result.IsSuccess)
            return new(result.Issue);
        return result.Map(await task);
    }

    public static async Task<Result<TOut>> Map<TOut>(this Task<Result> resultTask, TOut value)
    {
        var result = await resultTask;
        return result.Map(value);
    }

    public static async Task<Result<TOut>> Map<TOut>(this Task<Result> resultTask, Func<TOut> func)
    {
        var result = await resultTask;
        return Map(result, func);
    }

    public static async Task<Result<TOut>> MapAsync<TOut>(this Task<Result> resultTask, Task<TOut> task)
    {
        var result = await resultTask;
        return await MapAsync(result, task);
    }

    #endregion

    #region Map from a Result{TIn} to a Result{TOut} by providing a value.

    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TOut> func)
    {
        if (!result.IsSuccess)
            return new(result.Issue);
        return result.Map(func());
    }

    public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Result<TIn> result, Task<TOut> task)
    {
        if (!result.IsSuccess)
            return new(result.Issue);
        return result.Map(await task);
    }

    public static async Task<Result<TOut>> Map<TIn, TOut>(this Task<Result<TIn>> resultTask, TOut value)
    {
        var result = await resultTask;
        return result.Map(value);
    }

    public static async Task<Result<TOut>> Map<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TOut> func)
    {
        var result = await resultTask;
        return Map(result, func);
    }

    public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Task<Result<TIn>> resultTask, Task<TOut> task)
    {
        var result = await resultTask;
        return await MapAsync(result, task);
    }

    #endregion

    #region Map a Result{TIn} to Result{TOut} where TOut is Func(TIn)

    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> valueFunc)
    {
        if (!result.TryGetValue(out var value))
            return new(result.Issue);
        return result.Map(valueFunc(value));
    }

    public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> valueTaskFunc)
    {
        if (!result.TryGetValue(out var value))
            return new(result.Issue);
        return result.Map(await valueTaskFunc(value));
    }

    public static async Task<Result<TOut>> Map<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, TOut> valueFunc)
    {
        var result = await resultTask;
        return Map(result, valueFunc);
    }

    public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Task<Result<TIn>> resultTask, Func<TIn, Task<TOut>> valueTaskFunc)
    {
        var result = await resultTask;
        return await MapAsync(result, valueTaskFunc);
    }
    #endregion

    #region Map a Result{(T1, T2)} to a Result{TOut} where TOut if Func(T1, T2)

    public static Result<TOut> Map<T1, T2, TOut>(this Result<(T1, T2)> result, Func<T1, T2, TOut> valueFunc)
    {
        if (!result.TryGetValue(out var tuple))
            return new(result.Issue);
        var value = valueFunc(tuple.Item1, tuple.Item2);
        return result.Map(value);
    }

    public static async Task<Result<TOut>> MapAsync<T1, T2, TOut>(this Result<(T1, T2)> result, Func<T1, T2, Task<TOut>> valueTaskFunc)
    {
        if (!result.TryGetValue(out var tuple))
            return new(result.Issue);
        var value = await valueTaskFunc(tuple.Item1, tuple.Item2);
        return result.Map(value);
    }

    public static async Task<Result<TOut>> Map<T1, T2, TOut>(this Task<Result<(T1, T2)>> resultTask, Func<T1, T2, TOut> valueFunc)
    {
        var result = await resultTask;
        return Map(result, valueFunc);
    }

    public static async Task<Result<TOut>> MapAsync<T1, T2, TOut>(this Task<Result<(T1, T2)>> resultTask, Func<T1, T2, Task<TOut>> valueTaskFunc)
    {
        var result = await resultTask;
        return await MapAsync(result, valueTaskFunc);
    }

    #endregion
}
