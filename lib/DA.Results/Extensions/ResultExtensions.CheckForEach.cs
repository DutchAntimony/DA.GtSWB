namespace DA.Results.Extensions;

public static partial class ResultExtensions
{
    public static Result CheckForEach<TEnumeration>(this Result result, IEnumerable<TEnumeration> items, Func<TEnumeration, Result> predicate)
    {
        foreach (var item in items)
        {
            if (!result.IsSuccess) break;
            var itemResult = predicate(item);
            result = result.Check(itemResult);
        }
        return result;
    }
    public static Result<TValue> CheckForEach<TValue, TEnumeration>(this Result<TValue> result, IEnumerable<TEnumeration> enumeration, Func<TEnumeration, TValue, Result> predicate)
    {
        foreach (var item in enumeration)
        {
            if (!result.TryGetValue(out var value)) break; // if at any moment the result is Failure, don't do more checks.
            var itemResult = predicate(item, value);
            result = result.Check(itemResult);
        }
        return result;
    }

    public static async Task<Result> CheckForEachAsync<TEnumeration>(this Result result, IEnumerable<TEnumeration> items, Func<TEnumeration, Task<Result>> predicateTask)
    {
        foreach (var item in items)
        {
            if (!result.IsSuccess) break; // if at any moment the result is Failure, don't do more checks.
            var itemResult = await predicateTask(item);
            result = result.Check(itemResult);
        }
        return result;
    }

    public static async Task<Result<TValue>> CheckForEach<TValue, TEnumeration>(
    this Task<Result<TValue>> resultTask, IEnumerable<TEnumeration> enumeration, Func<TEnumeration, TValue, Result> predicate)
    {
        var result = await resultTask;
        return CheckForEach(result, enumeration, predicate);
    }

    public static async Task<Result<TValue>> CheckForEachAsync<TValue, TEnumeration>(
        this Result<TValue> result, IEnumerable<TEnumeration> enumeration, Func<TEnumeration, TValue, Task<Result>> predicateTask)
    {
        foreach (var item in enumeration)
        {
            if (!result.TryGetValue(out var value)) break; // if at any moment the result is Failure, don't do more checks.
            var itemResult = await predicateTask(item, value);
            result = result.Check(itemResult);
        }
        return result;
    }

    public static async Task<Result<TValue>> CheckForEachAsync<TValue, TEnumeration>(
        this Task<Result<TValue>> resultTask, IEnumerable<TEnumeration> enumeration, Func<TEnumeration, TValue, Task<Result>> predicateTask)
    {
        var result = await resultTask;
        return await CheckForEachAsync(result, enumeration, predicateTask);
    }

    public static async Task<Result<IEnumerable<TValue>>> CheckForEachAsync<TValue>(this Task<Result<IEnumerable<TValue>>> resultTask, Func<TValue, Task<Result>> checkTask)
    {
        var result = await resultTask;
        if (!result.TryGetValue(out var collection)) return result;
        foreach (var item in collection)
        {
            var innerResult = await checkTask(item);
            if (!innerResult.IsSuccess) return result.Check(innerResult);
        }
        return result;
    }

    public static async Task<Result<IEnumerable<TValue>>> CheckForEach<TValue>(this Task<Result<IEnumerable<TValue>>> resultTask, Func<TValue, Result> checkTask)
    {
        var result = await resultTask;
        if (!result.TryGetValue(out var collection)) return result;
        foreach (var item in collection)
        {
            var innerResult = checkTask(item);
            if (!innerResult.IsSuccess) return result.Check(innerResult);
        }
        return result;
    }
}
