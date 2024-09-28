using DA.GtSWB.Common.Types.IDs;

namespace DA.GtSWB.Domain.Models.Ledenadministratie.Betaalwijzes;

public class IncassoBetaalwijze : Betaalwijze
{
    public required Bankrekening Bankrekening { get; set; }

    private IncassoBetaalwijze() { }

    public static IncassoBetaalwijze Create(Bankrekening bankrekening)
    {
        return new()
        {
            Id = BetaalwijzeId.Create(),
            Bankrekening = bankrekening
        };
    }
}
