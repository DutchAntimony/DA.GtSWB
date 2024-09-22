using DA.GtSWB.Common.Data.IDs;
using DA.GtSWB.Domain.Extensions;
using DA.GtSWB.Domain.ServiceDefinitions;

namespace DA.GtSWB.Domain.Models.Ledenadministratie;

public class Lid
{
    public LidId Id { get; init; } = LidId.Empty;
    public int Lidnummer { get; init; }
    public Personalia Personalia { get; private set; } = null!;

    public Adres? AdresDataDatabase { get; set; } = null;
    public Adres Adres => AdresDataDatabase ?? Adres.Empty;

    //internal BetaalwijzeData? BetaalwijzeData { get; private set; } = null;
    //public BetaalwijzeData Betaalwijze => BetaalwijzeData ?? Betaalwijzes.Betaalwijze.Gratis;

    //public bool IsUitgeschreven { get; private set; } = false;

    private Lid() { }

    public static Lid Create(
        ILidnummerProvider lidnummerProvider,
        Personalia personalia,
        Option<Adres> adres
        //Option<BetaalwijzeData> betaalwijze,
        //DateTime referentiedatum
        )
    {
        return new Lid
        {
            Id = LidId.Create(),
            Lidnummer = lidnummerProvider.GetNext(),
            Personalia = personalia,
            AdresDataDatabase = adres.ToNullIf(a => a.Id.IsEmpty()),
            //BetaalwijzeData = betaalwijze.ToNullIf(b => b.Id.IsEmpty())
        };
    }

    //public static void ApplyNaamWijzigingMutatie(NaamWijzigingMutatie mutatie)
    //{
    //    mutatie.Lid.UpdatePersonalia(mutatie.NieuwPersonalia);
    //}

    private void UpdatePersonalia(Personalia newPersoonData)
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

    //public static void ApplyVerhuisMutatie(VerhuisMutatie mutatie)
    //{
    //    mutatie.Lid.Verhuis(mutatie.NieuwAdres.AsOption());
    //    if (mutatie.NieuweBetaalwijze is not null)
    //    {
    //        mutatie.Lid.BetaalwijzeData = mutatie.NieuweBetaalwijze;
    //    }
    //}

    private void Verhuis(Option<Adres> newAdres)
    {
        var adres = newAdres.ToNullIf(a => a.Id.IsEmpty());

        if (adres?.Id == AdresDataDatabase?.Id)
        {
            return;
        }

        AdresDataDatabase = adres;
    }

    //public static void ApplyBetaalwijzeMutatie(BetaalwijzeMutatie mutatie)
    //{
    //    mutatie.Lid.UpdateBetaalwijze(mutatie.NieuweBetaalwijze);
    //}

    //private void UpdateBetaalwijze(BetaalwijzeData? betaalwijze)
    //{
    //    BetaalwijzeData = betaalwijze;
    //}

    //public static void ApplyUitschrijfMutatie(UitschrijfMutatie mutatie)
    //{
    //    mutatie.Lid.SchrijfUit();
    //}
    //private void SchrijfUit()
    //{
    //    IsUitgeschreven = true;
    //}
}
