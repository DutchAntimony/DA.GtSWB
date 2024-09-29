using DA.GtSWB.Domain.Models.Ledenadministratie;

namespace DA.GtSWB.Domain.Models.Contributie.NotaOpdrachtRegels;

public class LidContributieNotaRegel : OpdrachtRegel
{
    public required Lid Lid { get; init; }
}

