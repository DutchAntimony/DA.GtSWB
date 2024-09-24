namespace DA.Results.Issues;

public class ValidationFailureCollection
{
    private readonly List<ValidationFailure> _failures = [];
    private Issue? _lastEncounteredError;

    public ValidationFailureCollection AddFrom(Result result)
    {
        result.Act()
            .OnSuccess(DoNothing)
            .OnError<ValidationError>(error => _failures.AddRange(error.Failures))
            .OnDefaultError(error => _lastEncounteredError = error)
            .Execute();
        return this;
    }

    public Result ToResult()
    {
        if (_lastEncounteredError is not null)
            return new(_lastEncounteredError);

        if (_failures.Count != 0)
            return new ValidationError(_failures);

        return new Result();
    }

    public Result<TValue> ToResult<TValue>(TValue value)
    {
        if (_lastEncounteredError is not null)
            return new(_lastEncounteredError);

        if (_failures.Count != 0)
            return new ValidationError(_failures);

        return new Result<TValue>(value);
    }

    private void DoNothing() { }
}