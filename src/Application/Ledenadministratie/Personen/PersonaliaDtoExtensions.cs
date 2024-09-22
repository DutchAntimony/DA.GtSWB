using DA.GtSWB.Application.Ledenadministratie.Personen;
using DA.GtSWB.Common.Data.Enums;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using System.Text.RegularExpressions;

namespace DA.GtSWB.Application.Leden.Personen;

public static partial class PersonaliaDtoExtensions
{
    internal static Personalia ToDomainModel(this PersonaliaDto dto)
    {
        return Personalia.Create(
            dto.Voorletters,
            dto.Tussenvoegsel.AsOption(),
            dto.Achternaam,
            dto.Geslacht.Parse(),
            dto.Geboortedatum);
    }

    internal static PersonaliaDto Sanitize(this PersonaliaDto dto)
    {
        return dto with
        {
            IsSanitized = true,
            Voorletters = SanitizeVoorletters(dto.Voorletters),
            Achternaam = dto.Achternaam.Sanitize(),
            Geslacht = dto.Geslacht.Sanitize(),
        };
    }

    private static Geslacht Parse(this string value)
    {
        return value switch
        {
            "M" or "Man" or "Dhr" or "Dhr." => Geslacht.Man,
            "V" or "Vrouw" or "Mevr" or "Mevr." => Geslacht.Vrouw,
            _ => Geslacht.Onbekend
        };
    }

    private static string SanitizeVoorletters(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        input = input.Replace(".", "").Replace(" ", "").Normalize();

        var regexPattern = HasAllLowerLetters().IsMatch(input) ? RegexLowercaseInput() : RegexMixedInput();

        // Match single initials and combined initials
        var matches = regexPattern.Matches(input);

        // If any invalid characters or combinations are found, return empty string
        if (matches.Count != 0 && string.Concat(matches).Length == input.Length)
        {
            // Build the sanitized initials
            return string.Join(".", matches.Select(m => string.Concat(m.ToString()[0].ToString().ToUpper(), m.ToString().AsSpan(1)))) + ".";
        }

        return string.Empty;
    }

    [GeneratedRegex(@"^[a-z]+$")]
    private static partial Regex HasAllLowerLetters();

    [GeneratedRegex(@"(Sj|Tj|Ch|Ij|Th|Pt|[A-Z])")]
    private static partial Regex RegexMixedInput();

    [GeneratedRegex(@"[a-z]")]
    private static partial Regex RegexLowercaseInput();
}