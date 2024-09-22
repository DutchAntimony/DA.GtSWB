using DA.GtSWB.Application.Ledenadministratie.Adressen;
using DA.GtSWB.Domain.Models.Ledenadministratie;

namespace DA.GtSWB.Application.Ledenadministratie.Adressen;

public static class AdresDtoExtensions
{
    public static Adres ToDomainModel(this AdresDto dto)
    {
        return Adres.Create(
            dto.Straat,
            dto.Huisnummer,
            dto.Postcode,
            dto.Woonplaats,
            dto.Land.AsOption());
    }

    internal static AdresDto Sanitize(this AdresDto dto)
    {
        return dto with
        {
            IsSanitized = true,
            Straat = dto.Straat.Sanitize(),
            Huisnummer = dto.Huisnummer.Sanitize(),
            Postcode = dto.Postcode.Replace(" ", "").ToUpperInvariant().Sanitize(),
            Woonplaats = dto.Woonplaats.Sanitize(),
            Land = SanitizeLand(dto.Land)
        };
    }

    private static string? SanitizeLand(string? land)
    {
        land = land?.Sanitize();
        return land switch
        {
            null or "" or "NL" or "Nederland" => null,
            "BE" or "Belgie" or "België" or "Belgique" or "Belgium" => "B",
            "DE" or "Duitsland" or "Deutschland" or "Germany" => "D",
            _ => land
        };
    }
}