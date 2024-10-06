using DA.GtSWB.Common.Types.Enums;

namespace DA.GtSWB.Common.Types.Extensions;

public static class MandaadTypeExtensions
{
    public static string GetIdentifier(this MandaadType type)
    {
        return type switch
        {
            MandaadType.GroeneKaart => "G",
            MandaadType.SepaMachtiging => "S",
            MandaadType.Email => "E",
            MandaadType.Geen => "X",
            _ => throw new NotImplementedException(),
        };
    }

    public static MandaadType FromIdentifier(string identifier)
    {
        return identifier switch
        {
            "G" => MandaadType.GroeneKaart,
            "S" => MandaadType.SepaMachtiging,
            "E" => MandaadType.Email,
            _ => MandaadType.Geen
        };
    }
}