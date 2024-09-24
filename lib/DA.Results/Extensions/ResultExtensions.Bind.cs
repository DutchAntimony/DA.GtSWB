namespace DA.Results.Extensions;

public static partial class ResultExtensions
{
    #region Bind Result to Result
    public static Result Bind(this Result first, Func<Result> secondFunc)
    {
        if (!first.IsSuccess)
            return first;
        var second = secondFunc();
        return first.Bind(second);
    }

    public static async Task<Result> BindAsync(this Result first, Task<Result> secondTask)
    {
        if (!first.IsSuccess)
            return first;
        var second = await secondTask;
        return first.Bind(second);
    }

    public static async Task<Result> Bind(this Task<Result> firstTask, Result second)
    {
        var first = await firstTask;
        return first.Bind(second);
    }

    public static async Task<Result> Bind(this Task<Result> firstTask, Func<Result> secondFunc)
    {
        var first = await firstTask;
        return first.Bind(secondFunc);
    }

    public static async Task<Result> BindAsync(this Task<Result> firstTask, Task<Result> secondTask)
    {
        var first = await firstTask;
        return await first.BindAsync(secondTask);
    }
    #endregion

    #region Bind Result to Result{T}
    public static Result<T> Bind<T>(this Result first, Func<Result<T>> secondFunc)
    {
        if (!first.IsSuccess)
            return new(first.Issue);
        var second = secondFunc();
        return first.Bind(second);
    }

    public static async Task<Result<T>> BindAsync<T>(this Result first, Task<Result<T>> secondTask)
    {
        if (!first.IsSuccess)
            return new(first.Issue);
        var second = await secondTask;
        return first.Bind(second);
    }

    public static async Task<Result<T>> Bind<T>(this Task<Result> firstTask, Result<T> second)
    {
        var first = await firstTask;
        return first.Bind(second);
    }

    public static async Task<Result<T>> Bind<T>(this Task<Result> firstTask, Func<Result<T>> secondFunc)
    {
        var first = await firstTask;
        return Bind(first, secondFunc);
    }

    public static async Task<Result<T>> BindAsync<T>(this Task<Result> firstTask, Task<Result<T>> secondTask)
    {
        var first = await firstTask;
        return await BindAsync(first, secondTask);
    }

    #endregion

    #region Bind Result{T} to Result
    /*
     * This region is empty. There is no reason to Bind a Result{T} to a Result.
     * To do a check on the Result{T} use the check method. 
     * To reduce a Result{T} to a Result, use the Flatten method.
     */

    #endregion

    #region Bind Result{TIn} to Result{TOut}
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> first, Func<TIn, Result<TOut>> secondFunc)
    {
        if (!first.TryGetValue(out var value))
            return new(first.Issue);
        var second = secondFunc(value);
        return first.Bind(second);
    }

    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> first, Task<Result<TOut>> secondTask)
    {
        if (!first.IsSuccess)
            return new(first.Issue);
        var second = await secondTask;
        return first.Bind(second);
    }

    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> first, Func<TIn, Task<Result<TOut>>> secondTaskFunc)
    {
        if (!first.TryGetValue(out var value))
            return new(first.Issue);
        var second = await secondTaskFunc(value);
        return first.Bind(second);
    }

    public static async Task<Result<TOut>> Bind<TIn, TOut>(this Task<Result<TIn>> firstTask, Result<TOut> second)
    {
        var first = await firstTask;
        return first.Bind(second);
    }

    public static async Task<Result<TOut>> Bind<TIn, TOut>(this Task<Result<TIn>> firstTask, Func<TIn, Result<TOut>> secondFunc)
    {
        var first = await firstTask;
        return Bind(first, secondFunc);
    }

    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> firstTask, Task<Result<TOut>> secondTask)
    {
        var first = await firstTask;
        return await BindAsync(first, secondTask);
    }

    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Task<Result<TIn>> firstTask, Func<TIn, Task<Result<TOut>>> secondTaskFunc)
    {
        var first = await firstTask;
        return await BindAsync(first, secondTaskFunc);
    }
    #endregion
}
