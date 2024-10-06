using DA.GtSWB.Common.Extensions;
using System.Text.RegularExpressions;

namespace DA.GtSWB.Common.Types;

public readonly record struct Iban
{
    public string Landcode { get; }
    public string ControleGetal { get; }
    public string BankIdentifier { get; }
    public string Rekeningnummer { get; }

    public readonly override string ToString() =>
        $"{Landcode} {ControleGetal} {BankIdentifier} {Regex.Replace(Rekeningnummer, ".{4}", "$0 ").Trim()}";

    public readonly string ToFlatString() =>
        $"{Landcode}{ControleGetal}{BankIdentifier}{Rekeningnummer}";

    public Iban(string value)
    {
        value = value.Replace(" ", "").ToUpperInvariant();
        Landcode = value[..2];
        ControleGetal = value.Substring(2, 2);
        BankIdentifier = value.Substring(4, 4);
        Rekeningnummer = value[8..];
    }

    public static Result<Iban> TryCreate(string value)
    {
        return new ValidationFailureCollection()
            .AddFrom(value.VerifyNotNull(nameof(Iban))
                .Bind(() => value.VerifyMinLength(15, nameof(Iban)))
                .Bind(() => value.VerifyMaxLength(34, nameof(Iban)))
                .Bind(() => ValidationFailureExtensions.InvalidIf(!Regex.IsMatch(value[..2], "^[A-Z]{2}$"), "Ongeldige iban. Controleer de invoer.", nameof(Iban)))
                .Bind(() => ValidationFailureExtensions.InvalidIf(!IsValidIban(value), "Ongeldige iban. Controleer de invoer.", nameof(Iban))))
            .ToResult().Map(new Iban(value));
    }

    private static bool IsValidIban(string iban)
    {
        // Rearrange IBAN: move the first four characters to the end
        var rearranged = string.Concat(iban.AsSpan(4), iban.AsSpan(0, 4));

        // Convert letters to numbers (A = 10, B = 11, ..., Z = 35)
        var numericIban = "";
        foreach (var c in rearranged)
        {
            if (char.IsLetter(c))
                numericIban += (c - 'A' + 10).ToString();
            else
                numericIban += c;
        }

        // Perform mod-97 operation on the numeric IBAN
        return Mod97(numericIban) == 1;
    }

    private static int Mod97(string numericIban)
    {
        // Use a sliding window to perform mod-97, as the IBAN number can be very large
        var remainder = 0;
        foreach (var c in numericIban)
        {
            remainder = (remainder * 10 + (c - '0')) % 97;
        }
        return remainder;
    }
}
