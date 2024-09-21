namespace DA.Results.Extensions;
public static partial class ResultExtensions
{
    /// <summary>
    /// If the current result is invalid, compensate by generating an alternative result.
    /// Looses information about the current failure.
    /// </summary>
    /// <param name="compensator">Function to generate a compensate for the current result.</param>
    public static Result Compensate(this Result result, Func<Result> compensator)
    {
        if (result.IsSuccess) return result;
        return compensator();
    }

    /// <summary>
    /// If the current result is invalid, compensate by generating an alternative result.
    /// Looses information about the current failure.
    /// </summary>
    /// <param name="compensator">Function to generate a compensate for the current result.</param>
    public static async Task<Result> CompensateAsync(this Result result, Func<Task<Result>> compensator)
    {
        if (result.IsSuccess) return result;
        return await compensator();
    }

    /// <summary>
    /// If the current result is invalid, compensate by generating an alternative result.
    /// Looses information about the current failure.
    /// </summary>
    /// <param name="compensator">Function to generate a compensate for the current result.</param>
    public static Result<TValue> Compensate<TValue>(this Result<TValue> result, Func<Result<TValue>> compensator)
    {
        if (result.IsSuccess) return result;
        return compensator();
    }
    /// <summary>
    /// If the current result is invalid, compensate by generating an alternative result.
    /// Looses information about the current failure.
    /// </summary>
    /// <param name="compensator">Function to generate a compensate for the current result.</param>
    public static async Task<Result<TValue>> CompensateAsync<TValue>(this Result<TValue> result, Func<Task<Result<TValue>>> compensator)
    {
        if (result.IsSuccess) return result;
        return await compensator();
    }

    /// <summary>
    /// If the current result is invalid, compensate by generating an alternative result.
    /// Looses information about the current failure.
    /// </summary>
    /// <param name="compensator">Function to generate a compensate for the current result.</param>
    public static async Task<Result> Compensate(this Task<Result> resultTask, Func<Result> compensator)
    {
        var result = await resultTask;
        return result.Compensate(compensator);
    }

    /// <summary>
    /// If the current result is invalid, compensate by generating an alternative result.
    /// Looses information about the current failure.
    /// </summary>
    /// <param name="compensator">Function to generate a compensate for the current result.</param>
    public static async Task<Result> Compensate(this Task<Result> resultTask, Func<Task<Result>> compensator)
    {
        var result = await resultTask;
        return await result.CompensateAsync(compensator);
    }

    /// <summary>
    /// If the current result is invalid, compensate by generating an alternative result.
    /// Looses information about the current failure.
    /// </summary>
    /// <param name="compensator">Function to generate a compensate for the current result.</param>
    public static async Task<Result<TValue>> Compensate<TValue>(this Task<Result<TValue>> resultTask, Func<Result<TValue>> compensator)
    {
        var result = await resultTask;
        return result.Compensate(compensator);
    }

    /// <summary>
    /// If the current result is invalid, compensate by generating an alternative result.
    /// Looses information about the current failure.
    /// </summary>
    /// <param name="compensator">Function to generate a compensate for the current result.</param>
    public static async Task<Result<TValue>> CompensateAsync<TValue>(this Task<Result<TValue>> resultTask, Func<Task<Result<TValue>>> compensator)
    {
        var result = await resultTask;
        return await result.CompensateAsync(compensator);
    }
}