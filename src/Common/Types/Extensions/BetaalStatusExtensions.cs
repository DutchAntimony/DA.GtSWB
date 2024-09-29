using DA.GtSWB.Common.Types.Enums;

namespace DA.GtSWB.Common.Types.Extensions;

public static class BetaalStatusExtensions
{
    private static readonly BetaalStatus[] _afgerondeBetaalStatussen =
        [BetaalStatus.Vervallen, BetaalStatus.Kwijtgescholden, BetaalStatus.Voldaan];

    public static bool IsAfgerond(this BetaalStatus betaalStatus) => _afgerondeBetaalStatussen.Contains(betaalStatus);

    public static bool IsGeldigeNieuweStatus(this BetaalStatus nieuw, BetaalStatus oud)
    {
        if (oud == nieuw)
            return false;

        return (oud, nieuw) switch
        {
            (BetaalStatus.Open, _) => true,
            (BetaalStatus.Teruggevorderd, BetaalStatus.Vervallen) => true,
            (BetaalStatus.Teruggevorderd, BetaalStatus.Voldaan) => true, // als het bedrag alsnog handmatig wordt overgemaakt.
            _ => false
        };
    }
}
