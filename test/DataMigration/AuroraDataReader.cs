using Microsoft.Data.Sqlite;
using Dapper;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using System.Globalization;
using DA.GtSWB.Common.Types;
using DA.GtSWB.Common.Types.Extensions;
using DA.GtSWB.Domain.Models.Ledenadministratie.Betaalwijzes;
using DA.GtSWB.Tests.DataMigration.AuroraDtos;
using DA.GtSWB.Tests.DataMigration.CsvImport;

namespace DataMigration;

internal static class AuroraDataReader
{
    internal static IEnumerable<AuroraLidDto> ReadLeden(string connectionString)
    {
        string query = "SELECT * FROM Lid;";

        var connection = new SqliteConnection(connectionString);
        connection.Open();
        return connection.Query<AuroraLidDto>(query);
    }

    internal static Dictionary<int, Adres> ReadAdressen(string connectionString)
    {
        string query = "SELECT * FROM Familie;";

        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var collection = connection.Query<AuroraAdresDto>(query);

        return collection.ToDictionary(a => a.Id, a => a.ToAdres());
    }

    internal static Dictionary<int, IncassoBetaalwijze> ReadIncassoBetaalwijzes(string connectionString, string mandaadFile)
    {
        var result = new Dictionary<int, IncassoBetaalwijze>();

        var betaalwijzes = ReadBetaalwijzes(connectionString);
        var mandaten = ReadIncassoMandaten(mandaadFile);

        foreach (var betaalwijze in betaalwijzes)
        {
            var id = betaalwijze.Key;
            var ibanRaw = betaalwijze.Value.Iban!.Replace(" ", "");
            if (!mandaten.TryGetValue(ibanRaw, out var mandaad))
            {
                Console.WriteLine($"Geen mandaad voor Iban {ibanRaw}");
                mandaad = new(ibanRaw, "X", null);
            }

            var iban = Iban.TryCreate(ibanRaw).ReduceOrThrow();
            var bankrekening = Bankrekening.Create(iban, betaalwijze.Value.Bic!, betaalwijze.Value.TenNameVan!, mandaad.Datum, MandaadTypeExtensions.FromIdentifier(mandaad.Type));
            var incBetaalwijze = IncassoBetaalwijze.Create(bankrekening);
            result.Add(id, incBetaalwijze);
        }

        return result;
    }

    private static Dictionary<string, IncassoMandaadDto> ReadIncassoMandaten(string mandaadFile)
    {
        var dtos = new List<IncassoMandaadDto>();

        using var reader = new StreamReader(mandaadFile);

        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            var fields = line.Split(';');
            var iban = fields[0];
            var type = fields[3];
            var datum = DateOnly.ParseExact(fields[4], "dd/MM/yyyy", CultureInfo.InvariantCulture);

            dtos.Add(new(iban, type, datum));
        }
        return dtos.ToDictionary(dto => dto.Iban, dto => dto);
    }

    private static Dictionary<int, AuroraRekeningnummerDto> ReadBetaalwijzes(string connectionString)
    {
        string query = "SELECT * FROM Betaalwijze WHERE Betaalmethode = 0;";

        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var collection = connection.Query<AuroraRekeningnummerDto>(query);

        return collection.ToDictionary(r => r.Id, r => r);
    }
}