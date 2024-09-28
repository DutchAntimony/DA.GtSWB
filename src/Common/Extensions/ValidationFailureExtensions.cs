using System.Runtime.CompilerServices;

namespace DA.GtSWB.Common.Extensions;
public static class ValidationFailureExtensions
{
    public static Result<T> VerifyNotNull<T>(this T? value, [CallerMemberName] string propertyname = "") =>
        InvalidIf(value is null, $"{propertyname} is een verplicht veld.", propertyname)
        .Map(value!);

    public static Result VerifyNotEmpty(this string? value, [CallerMemberName] string propertyname = "") =>
        InvalidIf(string.IsNullOrWhiteSpace(value), $"{propertyname} is een verplicht veld.", propertyname);

    public static Result VerifyLengthBetween(this string? value, int minLength, int maxLength, [CallerMemberName] string propertyname = "") =>
        value.VerifyMinLength(minLength, propertyname).Bind(value.VerifyMaxLength(maxLength, propertyname));

    public static Result VerifyMinLength(this string? value, int minLength, [CallerMemberName] string propertyname = "") =>
        InvalidIf(value is not null && value.Length < minLength, $"Minimale lengte van {propertyname} is niet aan voldaan(min. lengte = {minLength} tekens.", propertyname);

    public static Result VerifyMaxLength(this string? value, int maxLength, [CallerMemberName] string propertyname = "") =>
        InvalidIf(value is not null && value.Length > maxLength, $"Maximale lengte van {propertyname} is overschreden (max. lengte = {maxLength} tekens.", propertyname);

    public static Result VerifyBefore(this DateTime value, DateTime other, [CallerMemberName] string propertyName = "") =>
        InvalidIf(value > other, $"Ongeldige {propertyName}, waarde mag niet na {other:dd/MM/yyyy} liggen.", propertyName);

    public static Result VerifyBefore(this DateOnly value, DateTime other, [CallerMemberName] string propertyName = "") =>
        value.ToDateTime(TimeOnly.MinValue).VerifyBefore(other, propertyName);

    public static Result VerifyLessThan(this int value, int comparedTo, [CallerMemberName] string propertyName = "") =>
        InvalidIf(value >= comparedTo, $"Ongeldige {propertyName}, waarde moet kleiner zijn dan {comparedTo}", propertyName);

    public static Result InvalidIf(bool isInvalid, string message, [CallerMemberName] string propertyName = "")
    {
        return isInvalid ? new ValidationError(propertyName, message) : Result.Create();
    }
}
