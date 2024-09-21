namespace DA.GtSWB.Common.Extensions;

public static class TypeExtensions
{
    public static string ReadableName(this Type type)
    {
        return type.FullName?.Split(".").Last().Replace('+', ' ') ?? type.Name;
    }
}