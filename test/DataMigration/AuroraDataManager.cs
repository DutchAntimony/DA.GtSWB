using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Domain.Models.Ledenadministratie.Betaalwijzes;
using DA.GtSWB.Persistence;

namespace DataMigration;

public static class AuroraDataManager
{
    public static async Task SaveLeden(LedenAdministratieDbContext dbContext, string connectionString, string mandaadFile)
    {
        var incBetaalwijzes = AuroraDataReader.ReadIncassoBetaalwijzes(connectionString, mandaadFile);
        var gebruikteBetaalwijzes = new Dictionary<int, IncassoBetaalwijze>();
        var notaBetaalwijzes = new Dictionary<AdresId, NotaBetaalwijze>();
        var adressen = AuroraDataReader.ReadAdressen(connectionString);
        var collection = AuroraDataReader.ReadLeden(connectionString);

        foreach (var dto in collection)
        {
            Console.WriteLine($"Verwerk import van lidnummer {dto.Lidnummer}");
            var adres = adressen[dto.FamilieId];
            var personalia = dto.ToPersonalia();
            var lid = Lid.ImportLid(dto.Lidnummer, personalia, adres, dto.UitschrijfredenId > 0, dto.WijzigingsDatum, "Aurora-Import");
            dbContext.Leden.Add(lid);
            await dbContext.SaveChangesAsync();

            if (dto.UitschrijfredenId > -1)
            {
                continue; // uitgeschreven lid betaald niet meer.
            } 

            if (dto.BetaalwijzeId == 1)
            {
                if (!notaBetaalwijzes.TryGetValue(adres.Id, out var value))
                {
                    value = NotaBetaalwijze.Create();
                    lid.AssignBetaalwijze(value, DateTime.Now, "Aurora-Import", true);
                    notaBetaalwijzes.Add(adres.Id, value);
                }
                else
                {
                    lid.AssignBetaalwijze(value, DateTime.Now, "Aurora-Import", false);
                }
            }
            if (dto.BetaalwijzeId == 2)
                continue; // gratis lid, geen betaalwijze;
            if (dto.BetaalwijzeId > 2)
            {
                if (!gebruikteBetaalwijzes.TryGetValue(dto.BetaalwijzeId, out var value))
                {
                    value = incBetaalwijzes[dto.BetaalwijzeId];
                    lid.AssignBetaalwijze(value, DateTime.Now, "Aurora-Import", true);
                    gebruikteBetaalwijzes.Add(dto.BetaalwijzeId, value);
                }
                else
                {
                    lid.AssignBetaalwijze(value, DateTime.Now, "Aurora-Import", false);
                }
            }
            if (dto.BetaalwijzeId < 1)
                throw new InvalidDataException();
        }
        await dbContext.SaveChangesAsync();
    }
}
