namespace DA.Results.Extensions;

public static partial class ResultExtensions
{
    public static async Task<Result> Flatten<T>(this Task<Result<T>> resultTask)
    {
        var result = await resultTask;
        return result.Flatten();
    }
}