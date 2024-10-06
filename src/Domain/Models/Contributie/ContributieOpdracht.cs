using DA.GtSWB.Common.Types;
using DA.GtSWB.Common.Types.Extensions;
using DA.GtSWB.Common.Types.IDs;

namespace DA.GtSWB.Domain.Models.Contributie;
public class ContributieOpdracht
{
    public required ContributieOpdrachtId Id { get; init; }
    public required int Jaargang { get; init; }

    public List<BetaalOpdracht> BetaalOpdrachtCollectie { get; init; } = [];
    public IEnumerable<BetaalOpdracht> BetaalOpdrachten => BetaalOpdrachtCollectie.AsEnumerable();

    public Money TotaalBedrag => BetaalOpdrachten.Sum(bo => bo.Bedrag);
    public Money OpenstaandBedrag => BetaalOpdrachten.Where(bo => !bo.IsAfgerond).Sum(bo => bo.Bedrag);

    public bool IsAfgerond => BetaalOpdrachten.All(bo => bo.IsAfgerond);

    public static ContributieOpdracht Create(int jaargang, IEnumerable<BetaalOpdracht> betaalopdrachten)
    {
        var opdracht = new ContributieOpdracht()
        {
            Id = ContributieOpdrachtId.Create(),
            Jaargang = jaargang
        };

        opdracht.BetaalOpdrachtCollectie.AddRange(betaalopdrachten);

        return opdracht;
    }
}
