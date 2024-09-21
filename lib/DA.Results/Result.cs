using DA.Results.Issues;

namespace DA.Results;

public class Result : IResult
{
    internal Issue Issue { get; }
    protected readonly bool _ignoreWarnings = false;

    /// <summary>
    /// Is the result a success?
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Does the result has an issue?
    /// </summary>
    public bool HasIssue => !IsSuccess;

    internal Result(Issue issue) =>
        (IsSuccess, Issue) = (false, issue);

    internal Result(bool ignoreWarnings = false) =>
        (IsSuccess, Issue, _ignoreWarnings) = (true, Issue.None, ignoreWarnings);

    /// <summary>
    /// Bind this and another result together.
    /// If any of the both results are errors, return an error result.
    /// In case of warnings, check the _ignoreWarnings flag on this result.
    /// </summary>
    /// <param name="other">The other result to bind.</param>
    /// <returns>A Result with the new status.</returns>
    public Result Bind(Result other)
    {
        if (HasIssue) return this;
        return other.Issue.Match(
            successValue: this,
            warningValue: _ignoreWarnings ? this : other,
            errorValue: other);
    }

    /// <summary>
    /// Bind this and a Result{<typeparamref name="TValue"/>} together.
    /// If any of the both results are errors, return an error result.
    /// In case of warnings, check the _ignoreWarnings flag on this result.
    /// </summary>
    /// <typeparam name="TValue">The type of the incoming result.</typeparam>
    /// <param name="other">The other result to bind.</param>
    /// <returns>A Result of <typeparamref name="TValue"/> with the new status.</returns>
    public Result<TValue> Bind<TValue>(Result<TValue> other)
    {
        if (HasIssue) return new(Issue);
        return other.Issue.Match(
            successValue: new(other._value!),
            warningValue: _ignoreWarnings ? new(other._value!) : other,
            errorValue: other);
    }

    /// <summary>
    /// Map this result to a Result{<typeparamref name="TValue"/>} by providing a value.
    /// If this result is an error, return an error result.
    /// </summary>
    /// <typeparam name="TValue">The type of the new result.</typeparam>
    /// <param name="value">The value of the result.</param>
    /// <returns>A Result of <typeparamref name="TValue"/> with the provided value.</returns>
    public Result<TValue> Map<TValue>(TValue value)
    {
        return IsSuccess
            ? new Result<TValue>(value, _ignoreWarnings)
            : new Result<TValue>(Issue);
    }

    public static implicit operator Result(Error issue) => new(issue);
    public static implicit operator Result(Warning warning) => new(warning);

    public static Result IgnoreWarnings(bool ignoreWarnings = true) => new(ignoreWarnings);
    public static Result Create() => new();
}
