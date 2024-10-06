using DA.GtSWB.Common.Types.IDs;

namespace DA.GtSWB.Domain.Models.Ledenadministratie.Betaalwijzes;

public class NotaBetaalwijze : Betaalwijze
{
    public bool PreferEmail { get; set; } = false;
    public Adres? VerstuurAdres => VerantwoordelijkLid?.Adres.AsNullable();

    private NotaBetaalwijze() { }

    public static NotaBetaalwijze Create(bool preferEmail = false)
    {
        return new NotaBetaalwijze()
        {
            Id = BetaalwijzeId.Create(),
            PreferEmail = preferEmail
        };
    }
}
