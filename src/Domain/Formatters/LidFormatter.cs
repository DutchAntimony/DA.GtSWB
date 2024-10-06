using DA.GtSWB.Common.Types.Extensions;
using DA.GtSWB.Domain.Models.Ledenadministratie;

namespace DA.GtSWB.Common.Formatters;

public delegate string LidFormatter(Lid lid);
public delegate string NaamFormatter(Personalia personalia);
public delegate string AdresFormatter(Adres adres);

public static class DefaultFormatters
{
    public static NaamFormatter VolledigeNaam =>
        (personalia) => $"{personalia.Geslacht.Format()}{personalia.Voorletters} {personalia.Tussenvoegsel.Map(t => t + " ").Reduce("")}{personalia.Achternaam}";

    public static AdresFormatter OpTweeRegels =>
        (adres) => $"{adres.Straat} {adres.Huisnummer}\n" +
        $"{adres.Postcode} {adres.Woonplaats.ToUpper()}";
}