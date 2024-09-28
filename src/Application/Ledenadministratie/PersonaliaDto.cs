using DA.GtSWB.Common.Types.Enums;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using System.Text.RegularExpressions;

namespace DA.GtSWB.Application.Ledenadministratie.Personen;

public partial record PersonaliaDto
{
    private string? _voorletters;
    public string? Voorletters
    {
        get => _voorletters;
        set => SetVoorletters(value);
    }

    private string? _tussenvoegsel;
    public string? Tussenvoegsel
    {
        get => _tussenvoegsel;
        set => _tussenvoegsel = value?.Sanitize();
    }

    private string? _achternaam;
    public string? Achternaam
    {
        get => _achternaam;
        set => _achternaam = value?.Sanitize();
    }

    private string? _geslacht;
    public string? GeslachtInput
    {
        get => _geslacht;
        set => _geslacht = value?.Sanitize();
    }

    public DateOnly Geboortedatum { get; set; } = DateOnly.MinValue;

    public Result<Personalia> ToDomainModel(DateTime currentDateTime)
    {
        return Validate(currentDateTime)
            .Map(Personalia.Create(Voorletters!, Tussenvoegsel.AsOption(), Achternaam!,
                 ParseGeslacht(GeslachtInput!), Geboortedatum));
    }

    public Result Validate(DateTime currentTimeStamp)
    {
        return new ValidationFailureCollection()
            .AddFrom(Voorletters.VerifyNotEmpty().Bind(Voorletters.VerifyMaxLength(15)))
            .AddFrom(Tussenvoegsel.VerifyMaxLength(15))
            .AddFrom(Achternaam.VerifyNotEmpty().Bind(Achternaam.VerifyLengthBetween(2, 127)))
            .AddFrom(GeslachtInput.VerifyNotNull())
            .AddFrom(Geboortedatum.VerifyNotNull().Bind(Geboortedatum.VerifyBefore(currentTimeStamp)))
            .AddFrom(Geboortedatum.GetAge(currentTimeStamp).VerifyLessThan(120))
            .ToResult();
    }

    private static Geslacht ParseGeslacht(string value)
    {
        return value switch
        {
            "M" or "Man" or "Dhr" or "Dhr." => Geslacht.Man,
            "V" or "Vrouw" or "Mevr" or "Mevr." => Geslacht.Vrouw,
            _ => Geslacht.Onbekend
        };
    }

    private void SetVoorletters(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            _voorletters = string.Empty;
            return;
        }

        input = input.Replace(".", "").Replace(" ", "").Normalize();

        var regexPattern = HasAllLowerLetters().IsMatch(input) ? RegexLowercaseInput() : RegexMixedInput();

        // Match single initials and combined initials
        var matches = regexPattern.Matches(input);

        // If any invalid characters or combinations are found, return empty string
        if (matches.Count != 0 && string.Concat(matches).Length == input.Length)
        {
            // Build the sanitized initials
            _voorletters = string.Join(".", matches.Select(m => string.Concat(m.ToString()[0].ToString().ToUpper(), m.ToString().AsSpan(1)))) + ".";
            return;
        }

        _voorletters = string.Empty;
    }

    [GeneratedRegex(@"^[a-z]+$")]
    private static partial Regex HasAllLowerLetters();

    [GeneratedRegex(@"(Sj|Tj|Ch|Ij|Th|Pt|[A-Z])")]
    private static partial Regex RegexMixedInput();

    [GeneratedRegex(@"[a-z]")]
    private static partial Regex RegexLowercaseInput();
}
