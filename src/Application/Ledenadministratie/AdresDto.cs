using DA.GtSWB.Domain.Models.Ledenadministratie;
using System.Text.RegularExpressions;

namespace DA.GtSWB.Application.Ledenadministratie.Adressen;

public partial record AdresDto
{
    private string? _straat;
    public string? Straat
    {
        get => _straat;
        set => _straat = value?.Sanitize();
    }

    private string? _huisnummer;
    public string? Huisnummer
    {
        get => _huisnummer;
        set => _huisnummer = value?.Sanitize();
    }

    private string? _postcode;
    public string? Postcode
    {
        get => _postcode;
        set => _postcode = value?.Replace(" ", "").ToUpperInvariant().Sanitize();
    }

    private string? _woonplaats;
    public string? Woonplaats
    {
        get => _woonplaats;
        set => _woonplaats = value?.Sanitize();
    }

    private string _landcode = string.Empty;
    public string? Land
    {
        get => _landcode;
        set => SetLandcode(value);
    }

    public Result<Adres> ToDomainModel()
    {
        return Validate()
            .Map(Adres.Create(Straat!, Huisnummer!, Postcode!, Woonplaats!,
            string.IsNullOrEmpty(Land) ? Option.None : Land.AsOption()));
    }

    public Result Validate()
    {
        return new ValidationFailureCollection()
            .AddFrom(Straat.VerifyNotEmpty(nameof(Straat)).Bind(Straat.VerifyLengthBetween(2, 100, nameof(Straat))))
            .AddFrom(Huisnummer.VerifyNotEmpty(nameof(Huisnummer)).Bind(Huisnummer.VerifyMaxLength(10, nameof(Huisnummer))))
            .AddFrom(Woonplaats.VerifyNotEmpty(nameof(Woonplaats)).Bind(Woonplaats.VerifyMaxLength(100, nameof(Woonplaats))))
            .AddFrom(Land.VerifyMaxLength(10, nameof(Land)))
            .AddFrom(Postcode.VerifyNotNull(nameof(Postcode)).Bind(() => ValidatePostcode(nameof(Postcode))))
            .ToResult();
    }

    private Result ValidatePostcode(string propertyName)
    {
        return Land switch
        {
            "" => Postcode.VerifyNotEmpty(propertyName)
                .Bind(ValidationFailureExtensions.InvalidIf(!NlPostcode().IsMatch(Postcode!),
                    "Voer een geldige Nederlandse postcode in of pas het land aan.", propertyName)),
            "B" => Postcode.VerifyNotEmpty(propertyName)
                .Bind(ValidationFailureExtensions.InvalidIf(!BPostcode().IsMatch(Postcode!),
                    "Voer een geldige Belgische postcode in of pas het land aan.", propertyName)),
            "D" => Postcode.VerifyNotEmpty(propertyName)
                .Bind(ValidationFailureExtensions.InvalidIf(!DPostcode().IsMatch(Postcode!),
                    "Voer een geldige Duitse postcode in of pas het land aan.", propertyName)),
            _ => Postcode.VerifyNotEmpty(propertyName).Bind(Postcode.VerifyLengthBetween(3, 10, propertyName))
        };
    }

    private void SetLandcode(string? land)
    {
        land = land?.Sanitize();
        _landcode = land switch
        {
            null or "" or "NL" or "Nederland" => string.Empty,
            "BE" or "Belgie" or "België" or "Belgique" or "Belgium" => "B",
            "DE" or "Duitsland" or "Deutschland" or "Germany" => "D",
            _ => land
        };
    }

    [GeneratedRegex(@"^(?:NL-)?(?:[1-9]\d{3} ?(?:[A-EGHJ-NPRTVWXZ][A-EGHJ-NPRSTVWXZ]|S[BCEGHJ-NPRTVWXZ]))$")]
    private static partial Regex NlPostcode();
    [GeneratedRegex(@"^(?:(?:[1-9])(?:\d{3}))$")]
    private static partial Regex BPostcode();
    [GeneratedRegex(@"^\d{5}$")]
    private static partial Regex DPostcode();
}