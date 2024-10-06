using DA.GtSWB.Common.Types;
using DA.GtSWB.Common.Types.Enums;
using DA.GtSWB.Common.Types.Extensions;
using DA.GtSWB.Common.Types.IDs;

namespace DA.GtSWB.Domain.Models.Contributie;

public abstract class BetaalOpdracht
{
    public BetaalOpdrachtId Id { get; init; }
    public required DateTime AanmaakDatum { get; init; }
    public required int Ident { get; init; }
    public Money Bedrag => OpdrachtRegels.Sum(nor => nor.Bedrag);
    public virtual int Iteratie { get; init; } = 1;
    public BetaalStatus Status { get; protected set; } = BetaalStatus.Open;
    public bool IsAfgerond => Status.IsAfgerond();


    public ContributieOpdrachtId ContributieOpdrachtId { get; init; }

    public List<OpdrachtRegel> OpdrachtRegelCollectie { get; init; } = [];
    public IEnumerable<OpdrachtRegel> OpdrachtRegels => OpdrachtRegelCollectie.AsEnumerable();

    protected void SetStatus(BetaalStatus newStatus)
    {
        Status = Status.IsGeldigeNieuweStatus(newStatus) ? newStatus
            : throw new InvalidOperationException($"Nieuwe status {newStatus} is ongeldig voor betaalopdracht met Id {Id}");
    }
}

public static class BetaalOpdrachtExtensions
{
    public static T Match<T>(this BetaalOpdracht opdracht,
        Func<IncassoOpdracht, T> matchIncasso,
        Func<NotaOpdracht, T> matchNota)
    {
        return opdracht switch
        {
            IncassoOpdracht inc => matchIncasso(inc),
            NotaOpdracht nota => matchNota(nota),
            _ => throw new InvalidOperationException()
        };
    }
}
