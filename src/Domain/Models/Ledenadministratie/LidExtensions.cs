namespace DA.GtSWB.Domain.Models.Ledenadministratie;

public static class LidExtensions
{
    public static Lid GetHoofdbewonerFromCollection(this IEnumerable<Lid> collection)
    {
        if (!collection.Any())
            throw new ArgumentNullException(nameof(collection));
        return collection.OrderBy(l => l.Lidnummer).First();
    }
}