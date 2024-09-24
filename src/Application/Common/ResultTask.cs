namespace DA.GtSWB.Application.Common;

internal static class ResultTask
{
    public static Task<Result<TValue>> Completed<TValue>(TValue value)
    {
        return Task.FromResult(Result.Create().Map(value));
    }
}