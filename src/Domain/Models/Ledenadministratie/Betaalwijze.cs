using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie.Betaalwijzes;

namespace DA.GtSWB.Domain.Models.Ledenadministratie;

public abstract class Betaalwijze
{
    public required BetaalwijzeId Id { get; init; }

    private ICollection<Lid> LidCollection { get; } = [];

    public IEnumerable<Lid> Leden => LidCollection;

    public LidId? VerantwoordelijkLidId { get; set; }
    public Lid? VerantwoordelijkLid { get; set; }

    internal static Betaalwijze Gratis => new GratisBetaalwijze() { Id = BetaalwijzeId.Empty };
}