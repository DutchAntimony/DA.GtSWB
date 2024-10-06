using DA.GtSWB.Common.Types.Enums;

namespace DA.GtSWB.Common.Types.Extensions;

public static class GeslachtExtensions
{
    public static string Format(this Geslacht geslacht)
    {
        return geslacht switch
        {
            Geslacht.Onbekend => string.Empty,
            Geslacht.Man => "Dhr. ",
            Geslacht.Vrouw => "Mevr. ",
            _ => throw new InvalidDataException(),
        };
    }
}