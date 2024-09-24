namespace DA.Results.Extensions;

// <summary>
/// Combine
/// Combines a Result{T1} and a Result{T2} to a Tuple Result{(T1, T2)}
/// </summary>
public static partial class ResultExtensions
{
    /// <summary>
    /// Combines a Result{T1} and a Result{T2} to a Tuple Result{(T1, T2)}
    /// </summary>
    public static Result<(T1, T2)> Combine<T1, T2>(this Result<T1> first, Func<T1, Result<T2>> secondFunc)
    {
        if (!first.TryGetValue(out var value))
            return new(first.Issue);
        var second = secondFunc(value);
        return first.Combine(second);
    }

    /// <summary>
    /// Combines a Result{T1} and a Result{T2} to a Tuple Result{(T1, T2)}
    /// </summary>
    public static async Task<Result<(T1, T2)>> CombineAsync<T1, T2>(this Result<T1> first, Task<Result<T2>> secondTask)
    {
        if (!first.IsSuccess)
            return new(first.Issue);
        var second = await secondTask;
        return first.Combine(second);
    }

    /// <summary>
    /// Combines a Result{T1} and a Result{T2} to a Tuple Result{(T1, T2)}
    /// </summary>
    public static async Task<Result<(T1, T2)>> CombineAsync<T1, T2>(this Result<T1> first, Func<T1, Task<Result<T2>>> secondTaskFunc)
    {
        if (!first.TryGetValue(out var value))
            return new(first.Issue);
        var second = await secondTaskFunc(value);
        return first.Combine(second);
    }

    /// <summary>
    /// Combines a Result{T1} and a Result{T2} to a Tuple Result{(T1, T2)}
    /// </summary>
    public static async Task<Result<(T1, T2)>> Combine<T1, T2>(this Task<Result<T1>> firstTask, Result<T2> second)
    {
        var first = await firstTask;
        return first.Combine(second);
    }

    /// <summary>
    /// Combines a Result{T1} and a Result{T2} to a Tuple Result{(T1, T2)}
    /// </summary>
    public static async Task<Result<(T1, T2)>> Combine<T1, T2>(this Task<Result<T1>> firstTask, Func<T1, Result<T2>> secondFunc)
    {
        var first = await firstTask;
        return Combine(first, secondFunc);
    }

    /// <summary>
    /// Combines a Result{T1} and a Result{T2} to a Tuple Result{(T1, T2)}
    /// </summary>
    public static async Task<Result<(T1, T2)>> CombineAsync<T1, T2>(this Task<Result<T1>> firstTask, Task<Result<T2>> secondTask)
    {
        var first = await firstTask;
        return await CombineAsync(first, secondTask);
    }

    /// <summary>
    /// Combines a Result{T1} and a Result{T2} to a Tuple Result{(T1, T2)}
    /// </summary>
    public static async Task<Result<(T1, T2)>> CombineAsync<T1, T2>(this Task<Result<T1>> firstTask, Func<T1, Task<Result<T2>>> secondTaskFunc)
    {
        var first = await firstTask;
        return await CombineAsync(first, secondTaskFunc);
    }
}