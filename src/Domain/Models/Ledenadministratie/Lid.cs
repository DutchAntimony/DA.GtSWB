using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Extensions;
using DA.GtSWB.Domain.ServiceDefinitions;

namespace DA.GtSWB.Domain.Models.Ledenadministratie;

public class Lid
{
    public LidId Id { get; init; } = LidId.Empty;
    public int Lidnummer { get; init; }
    public Personalia Personalia { get; private set; } = null!;

    public Adres? AdresDataDatabase { get; private set; } = null;
    public Option<Adres> Adres => AdresDataDatabase.AsOption();

    public BetaalwijzeId? BetaalwijzeId { get; set; } = null;

    public Betaalwijze? BetaalwijzeDataDatabase { get; private set; } = null;

    public Option<Betaalwijze> Betaalwijze => BetaalwijzeDataDatabase.AsOption();

    public bool IsUitgeschreven { get; private set; } = false;

    private Lid() { }

    public static async Task<Lid> Create(ILidnummerProvider lidnummerProvider,
        Personalia personalia, Adres adres, CancellationToken cancellationToken = default)
    {
        return new Lid
        {
            Id = LidId.Create(),
            Lidnummer = await lidnummerProvider.GetNextAsync(cancellationToken),
            Personalia = personalia,
            AdresDataDatabase = adres,
            BetaalwijzeDataDatabase = null
        };
    }

    public void AssignBetaalwijze(Betaalwijze betaalwijze, bool isResponsable = false)
    {
        BetaalwijzeDataDatabase = betaalwijze;

        // If this member is responsible for the payment method, set it
        if (isResponsable)
        {
            betaalwijze.VerantwoordelijkLid = this;
        }
    }

    public void UpdatePersonalia(Personalia newPersoonData)
    {
        if (newPersoonData.Id.IsEmpty())
        {
            throw new ArgumentNullException(nameof(newPersoonData));
        }

        if (newPersoonData.Id == Personalia.Id)
        {
            return;
        }

        Personalia = newPersoonData;
    }

    public void Verhuis(Option<Adres> newAdres)
    {
        var adres = newAdres.ToNullIf(a => a.Id.IsEmpty());

        if (adres?.Id == AdresDataDatabase?.Id)
        {
            return;
        }

        AdresDataDatabase = adres;
    }

    private void SchrijfUit()
    {
        IsUitgeschreven = true;
    }
}
