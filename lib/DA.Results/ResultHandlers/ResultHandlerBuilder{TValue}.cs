using DA.Results.Issues;

namespace DA.Results.ResultHandlers;

public class ResultHandlerBuilder<TValue>(Result<TValue> result)
{
    private Action<TValue>? _successHandler;
    private readonly Dictionary<Type, Action<object>> _warningHandlers = [];
    private Action<Warning>? _defaultWarningHandler;
    private Action<Issue>? _defaultIssueHandler;
    private readonly Dictionary<Type, Action<object>> _errorHandlers = [];

    public ResultHandlerBuilder<TValue> OnSuccess(Action<TValue> handler)
    {
        _successHandler = handler;
        return this;
    }

    public ResultHandlerBuilder<TValue> OnWarning<TWarning>(Action<TWarning> handler)
         where TWarning : Warning
    {
        _warningHandlers[typeof(TWarning)] = obj => handler((TWarning)obj);
        return this;
    }

    public ResultHandlerBuilder<TValue> OnError<TError>(Action<TError> handler)
        where TError : Issue
    {
        _errorHandlers[typeof(TError)] = obj => handler((TError)obj);
        return this;
    }

    public ResultHandlerBuilder<TValue> OnDefaultWarning(Action<Warning> handler)
    {
        _defaultWarningHandler = handler;
        return this;
    }

    public ResultHandlerBuilder<TValue> OnDefaultError(Action<Issue> handler)
    {
        _defaultIssueHandler = handler;
        return this;
    }

    public void Execute()
    {
        switch (result)
        {
            case { IsSuccess: true } when result.TryGetValue(out var value):
                _successHandler?.Invoke(value);
                break;

            case { Issue: Warning warning } when _warningHandlers.TryGetValue(warning.GetType(), out var warningHandler):
                warningHandler(warning);
                break;

            case { Issue: Warning warning } when _defaultWarningHandler is not null:
                _defaultWarningHandler.Invoke(warning);
                break;

            case { Issue: Error error } when _errorHandlers.TryGetValue(error.GetType(), out var errorHandler):
                errorHandler(error);
                break;

            default:
                _defaultIssueHandler?.Invoke(result.Issue);
                break;
        }
    }
}
