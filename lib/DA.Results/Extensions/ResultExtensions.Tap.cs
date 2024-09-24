namespace DA.Results.Extensions;

/// <summary>
/// Tap
/// Applies an Action{T} on the Result{T}, but only if the Result is a success result.
/// </summary>
public static partial class ResultExtensions
{
    #region Result without value to action
    public static TResult Tap<TResult>(this TResult result, Action action)
        where TResult : Result
    {
        if (result.IsSuccess)
            action();
        return result;
    }

    public static async Task<TResult> TapAsync<TResult>(this TResult result, Task task)
        where TResult : Result
    {
        if (result.IsSuccess)
            await task;
        return result;
    }

    public static async Task<TResult> Tap<TResult>(this Task<TResult> resultTask, Action action)
        where TResult : Result
    {
        var result = await resultTask;
        return result.Tap(action);
    }

    public static async Task<TResult> TapAsync<TResult>(this Task<TResult> resultTask, Task task)
    where TResult : Result
    {
        var result = await resultTask;
        return await result.TapAsync(task);
    }

    #endregion

    #region Result{T} to action(T)
    public static Result<TValue> Tap<TValue>(this Result<TValue> result, Action<TValue> action)
    {
        if (result.TryGetValue(out var value))
            action(value);
        return result;
    }

    public static async Task<Result<TValue>> TapAsync<TValue>(this Result<TValue> result, Func<TValue, Task> actionTask)
    {
        if (result.TryGetValue(out var value))
            await actionTask(value);
        return result;
    }

    public static async Task<Result<TValue>> Tap<TValue>(this Task<Result<TValue>> resultTask, Action<TValue> action)
    {
        var result = await resultTask;
        return result.Tap(action);
    }

    public static async Task<Result<TValue>> TapAsync<TValue>(this Task<Result<TValue>> resultTask, Func<TValue, Task> actionTask)
    {
        var result = await resultTask;
        return await result.TapAsync(actionTask);
    }
    #endregion

    #region Result{(T1,T2)} to action(T1,T2)
    public static Result<(T1, T2)> Tap<T1, T2>(this Result<(T1, T2)> result, Action<T1, T2> action)
    {
        if (result.TryGetValue(out var value))
        {
            action(value.Item1, value.Item2);
        }
        return result;
    }

    public static async Task<Result<(T1, T2)>> TapAsync<T1, T2>(this Result<(T1, T2)> result, Func<T1, T2, Task> actionTask)
    {
        if (result.TryGetValue(out var value))
        {
            await actionTask(value.Item1, value.Item2);
        }
        return result;
    }

    public static async Task<Result<(T1, T2)>> Tap<T1, T2>(this Task<Result<(T1, T2)>> resultTask, Action<T1, T2> action)
    {
        var result = await resultTask;
        return result.Tap(action);
    }

    public static async Task<Result<(T1, T2)>> TapAsync<T1, T2>(this Task<Result<(T1, T2)>> resultTask, Func<T1, T2, Task> actionTask)
    {
        var result = await resultTask;
        return await result.TapAsync(actionTask);
    }
    #endregion
}