using DA.Results.Issues;
using System.Diagnostics.CodeAnalysis;

namespace DA.Results;

public sealed class Result<TValue> : Result
{
    internal readonly TValue? _value;

    internal Result(Issue issue) : base(issue) { }

    internal Result(TValue value, bool ignoreWarnings = false) : base(ignoreWarnings) => _value = value;

    internal Result(TValue value, Warning warning) : base(warning) => _value = value;

    public bool TryGetValue([NotNullWhen(true), MaybeNullWhen(false)] out TValue value)
    {
        value = _value is not null ? _value : default;
        return IsSuccess;
    }

    internal bool TryGetSuccessOrWarningValue([NotNullWhen(true), MaybeNullWhen(false)] out TValue value)
    {
        value = _value is not null ? _value : default;
        return IsSuccess || Issue is Warning;
    }

    /// <summary>
    /// Check another result without changes the value of the current result.
    /// </summary>
    /// <param name="other">The other result to check.</param>
    /// <returns>This result with an updated issue if the other result had an issue.</returns>
    public Result<TValue> Check(Result other)
    {
        return HasIssue
            ? this
            : other.Issue.Match(
            successValue: this,
            warningValue: _ignoreWarnings ? this : new(other.Issue),
            errorValue: new(other.Issue));
    }

    /// <summary>
    /// Combine the results of this and another result, both in issue and in value (as tuple)
    /// If this result HasIssues, outcoming result is a failure with our issue.
    /// If other result HasIssues, outcoming result is a failure with other's issue.
    /// Else, SuccessResult with values as tuple.
    /// </summary>
    /// <typeparam name="TOther">The type of the value of the other result.</typeparam>
    /// <param name="other">The other result to comine with.</param>
    /// <returns>New result with a value type the combined values as tuple.</returns>
    public Result<(TValue, TOther)> Combine<TOther>(Result<TOther> other)
    {
        return HasIssue
            ? (Result<(TValue, TOther)>)new(Issue)
            : other.Issue.Match(
            successValue: Combine(other._value!),
            warningValue: _ignoreWarnings ? Combine(other._value!) : new(other.Issue),
            errorValue: new(other.Issue));
    }

    /// <summary>
    /// Combine the results of this and another value (as tuple)
    /// If this result HasIssue, outcoming result is a failure with that issue.
    /// Else, SuccessResult with values as tuple.
    /// </summary>
    /// <typeparam name="TOther">The type of the value to combine with.</typeparam>
    /// <param name="other">The other value to combine with.</param>
    /// <returns>New result with a value type the combined values as tuple.</returns>
    public Result<(TValue, TOther)> Combine<TOther>(TOther value) => HasIssue ? new(Issue) : new((_value!, value), _ignoreWarnings);

    public Result Flatten() => HasIssue ? new Result(Issue) : new Result(_ignoreWarnings);

    public static implicit operator Result<TValue>(Error error) => new(error);
    public static implicit operator Result<TValue>(TValue value) => new(value);

    public static Result<TValue> Create(TValue value) => new(value);
}