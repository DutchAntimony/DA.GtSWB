using FluentValidation;

namespace DA.GtSWB.Application.Ledenadministratie.Adressen;

public class AdresDtoValidator : AbstractValidator<AdresDto>
{
    private const string _nlRegex = @"^(?:NL-)?(?:[1-9]\d{3} ?(?:[A-EGHJ-NPRTVWXZ][A-EGHJ-NPRSTVWXZ]|S[BCEGHJ-NPRTVWXZ]))$";
    private const string _bRegex = @"^(?:(?:[1-9])(?:\d{3}))$";
    private const string _dRegex = @"^\d{5}$";

    public AdresDtoValidator()
    {
        string[] landCodesWithPostcodeRules = [string.Empty, "B", "D"];

        RuleFor(a => a).Must(a => a.IsSanitized)
            .WithMessage("Internal server error: Dto must be sanitized before validation");

        RuleFor(a => a.Straat).NotEmpty().Length(2, 100);
        RuleFor(a => a.Huisnummer).NotEmpty().MaximumLength(10);
        RuleFor(a => a.Woonplaats).NotEmpty().Length(2, 100);
        RuleFor(a => a.Land).NotNull();

        RuleFor(a => a.Postcode)
            .Matches(_nlRegex).When(a => a.Land == string.Empty)
            .WithMessage("Voer een geldige Nederlandse postcode in of pas het land aan.");

        RuleFor(a => a.Postcode)
            .Matches(_bRegex).When(a => a.Land == "B")
            .WithMessage("Voer een geldige Belgische postcode in of pas het land aan.");

        RuleFor(a => a.Postcode)
            .Matches(_dRegex).When(a => a.Land == "D")
            .WithMessage("Voer een geldige Duitse postcode in of pas het land aan.");

        RuleFor(a => a.Postcode)
            .Length(3, 10).When(a => !landCodesWithPostcodeRules.Contains(a.Land));
    }
}
