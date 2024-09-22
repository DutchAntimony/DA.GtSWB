using FluentValidation;

namespace DA.GtSWB.Application.Ledenadministratie.Personen;

public class PersoonValidator : AbstractValidator<PersonaliaDto>
{
    public PersoonValidator(DateTime? reference = null)
    {
        reference ??= DateTime.Now;

        RuleFor(p => p.Voorletters).NotEmpty().MaximumLength(15);
        RuleFor(p => p.Achternaam).NotEmpty().Length(2, 127);
        RuleFor(p => p.Geslacht).IsInEnum();
        RuleFor(p => p.Geboortedatum).NotEmpty();
        RuleFor(p => p.Geboortedatum)
            // moet in het verleden liggen (<= reference) en moet een leeftijd geven die <= 120 is (GetAge())
            .Must(geboortedatum => geboortedatum.GetAge(reference.Value) <= 120 && geboortedatum <= DateOnly.FromDateTime(reference.Value))
            .WithMessage("Voer een geldige geboortedatum in.");
    }
}

