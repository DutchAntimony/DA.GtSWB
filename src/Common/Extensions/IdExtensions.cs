using DA.GtSWB.Common.Data.IDs;

namespace DA.GtSWB.Common.Extensions;

public static class IdExtensions
{
    public static bool IsEmpty(this IId id)
    {
        return id is null || id.Value == Ulid.Empty;
    }
}