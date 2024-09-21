using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace DA.GtSWB.Common.Extensions;

public static class GuardExtensions
{
    /// <summary>
    /// Ensure that a given number has a strictly positive value.
    /// </summary>
    /// <typeparam name="TNumber">The type of the provided number.</typeparam>
    /// <param name="value">The value to ensure.</param>
    /// <param name="message">The message in the exception if the value is invalid.</param>
    /// <param name="parameter">Automatically filled; the name of the provided value.</param>
    /// <param name="method">Automatically filled; the name of the method calling this value.</param>
    /// <returns>Fluently the provided value, is value is ensured.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the ensure does not succeed.</exception>
    public static TNumber EnsurePositive<TNumber>(
        this TNumber value,
        string? message = null,
        [CallerArgumentExpression(nameof(value))] string parameter = "",
        [CallerMemberName] string method = "")
        where TNumber : INumber<TNumber> =>
        value > TNumber.Zero
            ? value
            : throw new ArgumentOutOfRangeException(parameter, value,
                message ??
                $"Ongeldige waarde {value} voor {parameter} in methode {method}. Waarde moet strikt positief zijn.");

    /// <summary>
    /// Ensure that a given number has a non negative (0 or greater) value.
    /// </summary>
    /// <typeparam name="TNumber">The type of the provided number.</typeparam>
    /// <param name="value">The value to ensure.</param>
    /// <param name="message">The message in the exception if the value is invalid.</param>
    /// <param name="parameter">Automatically filled; the name of the provided value.</param>
    /// <param name="method">Automatically filled; the name of the method calling this value.</param>
    /// <returns>Fluently the provided value, is value is ensured.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the ensure does not succeed.</exception>
    public static TNumber EnsureNotNegative<TNumber>(
        this TNumber value,
        string? message = null,
        [CallerArgumentExpression(nameof(value))] string parameter = "",
        [CallerMemberName] string method = "")
        where TNumber : INumber<TNumber> =>
        value >= TNumber.Zero
            ? value
            : throw new ArgumentOutOfRangeException(parameter, value,
                message ??
                $"Ongeldige waarde {value} voor {parameter} in methode {method}. Waarde mag niet negatief zijn.");

    /// <summary>
    /// Ensure that a given number is within the provided range.
    /// </summary>
    /// <typeparam name="TNumber">The type of the provided number.</typeparam>
    /// <param name="value">The value to ensure.</param>
    /// <param name="minValue">The minimum value for the result.</param>
    /// <param name="maxValue">The maximum value for the result.</param>
    /// <param name="message">The message in the exception if the value is invalid.</param>
    /// <param name="parameter">Automatically filled; the name of the provided value.</param>
    /// <param name="method">Automatically filled; the name of the method calling this value.</param>
    /// <returns>Fluently the provided value, is value is ensured.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the ensure does not succeed.</exception>
    public static TNumber EnsureInRange<TNumber>(
        this TNumber value,
        TNumber minValue,
        TNumber maxValue,
        string? message = null,
        [CallerArgumentExpression(nameof(value))] string parameter = "",
        [CallerMemberName] string method = "")
        where TNumber : INumber<TNumber> =>
        value >= minValue && value <= maxValue
            ? value
            : throw new ArgumentOutOfRangeException(parameter, value,
                message ??
                $"Ongeldige waarde {value} voor {parameter} in methode {method}. Waarde moet tussen {minValue} en {maxValue} liggen.");

    /// <summary>
    /// Ensure that a given Guid is not empty.
    /// </summary>
    /// <param name="value">The value to ensure.</param>
    /// <param name="message">The message in the exception if the value is invalid.</param>
    /// <param name="parameter">Automatically filled; the name of the provided value.</param>
    /// <param name="method">Automatically filled; the name of the method calling this value.</param>
    /// <returns>Fluently the provided value, is value is ensured.</returns>
    /// <exception cref="ArgumentException">Thrown if the ensure does not succeed.</exception>
    public static Guid EnsureNotEmpty(
        this Guid value,
        string? message = null,
        [CallerArgumentExpression(nameof(value))] string parameter = "",
        [CallerMemberName] string method = "") =>
        value != Guid.Empty
            ? value
            : throw new ArgumentException(parameter,
                message ??
                $"Ongeldige waarde {value} voor {parameter} in methode {method}. Guid mag niet leeg zijn.");

    /// <summary>
    /// Ensure that a given string is not empty.
    /// </summary>
    /// <param name="value">The value to ensure.</param>
    /// <param name="message">The message in the exception if the value is invalid.</param>
    /// <param name="parameter">Automatically filled; the name of the provided value.</param>
    /// <param name="method">Automatically filled; the name of the method calling this value.</param>
    /// <returns>Fluently the provided value, is value is ensured.</returns>
    /// <exception cref="ArgumentException">Thrown if the ensure does not succeed.</exception>
    public static string EnsureNotEmpty(
        this string? value,
        string? message = null,
        [CallerArgumentExpression(nameof(value))] string parameter = "",
        [CallerMemberName] string method = "") =>
        string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException(message ?? $"Ongeldige waarde {value} voor {parameter} in methode {method}. Waarde mag niet leeg zijn.", parameter)
            : value;

    /// <summary>
    /// Ensure that a given predicate is true.
    /// </summary>
    /// <param name="value">The value to ensure.</param>
    /// <param name="predicate">The function that must </param>
    /// <param name="message">The message in the exception if the value is invalid.</param>
    /// <param name="parameter">Automatically filled; the name of the provided value.</param>
    /// <param name="method">Automatically filled; the name of the method calling this value.</param>
    /// <returns>Fluently the provided value, is value is ensured.</returns>
    /// <exception cref="ArgumentException">Thrown if the ensure does not succeed.</exception>
    public static T EnsureTrue<T>(
        this T value,
        Func<T, bool> predicate,
        string? message,
        [CallerArgumentExpression(nameof(value))] string parameter = "") =>
        predicate(value)
            ? value
            : throw new ArgumentException(parameter, message);

    /// <summary>
    /// Ensure asynchronesly that a given predicate is true.
    /// </summary>
    /// <param name="value">The value to ensure.</param>
    /// <param name="predicate">The function that must </param>
    /// <param name="message">The message in the exception if the value is invalid.</param>
    /// <param name="parameter">Automatically filled; the name of the provided value.</param>
    /// <param name="method">Automatically filled; the name of the method calling this value.</param>
    /// <returns>Fluently the provided value, is value is ensured.</returns>
    /// <exception cref="ArgumentException">Thrown if the ensure does not succeed.</exception>
    public static async Task<T> EnsureTrueAsync<T>(
        this T value,
        Func<T, Task<bool>> predicate,
        string? message,
        [CallerArgumentExpression(nameof(value))] string parameter = "",
        [CallerMemberName] string method = "") =>
        await predicate(value)
            ? value
            : throw new ArgumentException(parameter, message);

    /// <summary>
    /// Ensure that the length of a given string is at least the provided threshold length.
    /// </summary>
    /// <param name="value">The value to ensure.</param>
    /// <param name="minLength">The minimum length of the provided string.</param>
    /// <param name="message">The message in the exception if the value is invalid.</param>
    /// <param name="parameter">Automatically filled; the name of the provided value.</param>
    /// <param name="method">Automatically filled; the name of the method calling this value.</param>
    /// <returns>Fluently the provided value, is value is ensured.</returns>
    /// <exception cref="ArgumentException">Thrown if the ensure does not succeed.</exception>
    public static string EnsureMinimumStringLength(
        this string? value,
        int minLength,
        string? message = null,
        [CallerArgumentExpression(nameof(value))] string parameter = "",
        [CallerMemberName] string method = "") =>
        value?.Length >= minLength
            ? value
            : throw new ArgumentException(parameter,
                message ?? $"Ongeldige waarde {value ?? "[null]"} voor {parameter} in methode {method}. Lengte is {value?.Length ?? 0} en moet minimaal {minLength} zijn.");


    /// <summary>
    /// Ensure that the length of a given string is not longer then provided threshold length.
    /// </summary>
    /// <param name="value">The value to ensure.</param>
    /// <param name="maxLength">The maximum length of the provided string.</param>
    /// <param name="message">The message in the exception if the value is invalid.</param>
    /// <param name="parameter">Automatically filled; the name of the provided value.</param>
    /// <param name="method">Automatically filled; the name of the method calling this value.</param>
    /// <returns>Fluently the provided value, is value is ensured.</returns>
    /// <exception cref="ArgumentException">Thrown if the ensure does not succeed.</exception>
    public static string EnsureMaximumStringLength(
        this string value,
        int maxLength,
        string? message = null,
        [CallerArgumentExpression(nameof(value))] string parameter = "",
        [CallerMemberName] string method = "") =>
        value.Length <= maxLength
            ? value
            : throw new ArgumentException(parameter,
                message ?? $"Ongeldige waarde {value} voor {parameter} in methode {method}. Lengte is {value.Length} en mag maximaal {maxLength} zijn.");

    /// <summary>
    /// Ensure that the provided value is not null.
    /// </summary>
    /// <param name="value">The value to ensure.</param>
    /// <param name="message">The message in the exception if the value is invalid.</param>
    /// <param name="parameter">Automatically filled; the name of the provided value.</param>
    /// <param name="method">Automatically filled; the name of the method calling this value.</param>
    /// <returns>Fluently the provided value, is value is ensured.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the ensure does not succeed.</exception>
    public static T EnsureNotNull<T>(
        [NotNull] this T? value,
        string? message = null,
        [CallerArgumentExpression(nameof(value))] string parameter = "",
        [CallerMemberName] string method = "") =>
        value is not null
            ? value
            : throw new ArgumentNullException(parameter,
                message ?? $"Ongeldige waarde voor {parameter} in methode {method}. Waarde mag niet null zijn.");

    /// <summary>
    /// Ensure that the provided value is a defined enum value.
    /// </summary>
    /// <param name="value">The value to ensure.</param>
    /// <param name="message">The message in the exception if the value is invalid.</param>
    /// <param name="parameter">Automatically filled; the name of the provided value.</param>
    /// <param name="method">Automatically filled; the name of the method calling this value.</param>
    /// <returns>Fluently the provided value, is value is ensured.</returns>
    /// <exception cref="ArgumentException">Thrown if the ensure does not succeed.</exception>
    public static TEnum EnsureDefined<TEnum>(
        this TEnum value,
        string? message = null,
        [CallerArgumentExpression(nameof(value))] string parameter = "",
        [CallerMemberName] string method = "")
        where TEnum : struct, Enum =>
        Enum.IsDefined(value)
            ? value
            : throw new ArgumentNullException(parameter,
                message ?? $"Ongeldige waarde {value} voor {parameter} in methode {method}. Waarde is onbekend.");
}