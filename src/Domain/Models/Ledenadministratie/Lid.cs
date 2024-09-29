using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Extensions;
using DA.GtSWB.Domain.Models.Ledenadministratie.Mutaties;
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

    public ICollection<LidMutatie> Mutaties { get; private set; } = [];

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

    public void AssignBetaalwijze(Betaalwijze betaalwijze, DateTime mutatieTimeStamp, string gebruiker, bool isResponsable = false)
    {
        Mutaties.Add(BetaalwijzeMutatie.Create(this, betaalwijze.AsOption(), mutatieTimeStamp, gebruiker));
        BetaalwijzeDataDatabase = betaalwijze;

        // If this member is responsible for the payment method, set it 
        if (isResponsable)
        {
            betaalwijze.VerantwoordelijkLid = this;
        }
    }

    public void UpdatePersonalia(Personalia newPersoonData, DateTime mutatieTimeStamp, string gebruiker)
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

        Mutaties.Add(NaamWijzigingMutatie.Create(this, newPersoonData, mutatieTimeStamp, gebruiker));
    }

    public void Verhuis(Option<Adres> newAdres, DateTime mutatieTimeStamp, string gebruiker)
    {
        var adres = newAdres.ToNullIf(a => a.Id.IsEmpty());

        if (adres?.Id == AdresDataDatabase?.Id)
        {
            return;
        }

        AdresDataDatabase = adres;

        Mutaties.Add(VerhuisMutatie.Create(this, newAdres, mutatieTimeStamp, gebruiker));
    }

    private void SchrijfUit()
    {
        IsUitgeschreven = true;
    }
}
