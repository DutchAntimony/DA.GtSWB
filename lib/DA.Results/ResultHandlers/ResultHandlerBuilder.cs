using DA.Results.Issues;

namespace DA.Results.ResultHandlers;

public class ResultHandlerBuilder(Result result)
{
    private Action? _successHandler;
    private readonly Dictionary<Type, Action<object>> _warningHandlers = [];
    private Action<Warning>? _defaultWarningHandler;
    private Action<Issue>? _defaultIssueHandler;
    private readonly Dictionary<Type, Action<object>> _errorHandlers = [];

    public ResultHandlerBuilder OnSuccess(Action handler)
    {
        _successHandler = handler;
        return this;
    }

    public ResultHandlerBuilder OnWarning<TWarning>(Action<TWarning> handler)
         where TWarning : Warning
    {
        _warningHandlers[typeof(TWarning)] = obj => handler((TWarning)obj);
        return this;
    }

    public ResultHandlerBuilder OnError<TError>(Action<TError> handler)
        where TError : Error
    {
        _errorHandlers[typeof(TError)] = obj => handler((TError)obj);
        return this;
    }

    public ResultHandlerBuilder OnDefaultWarning(Action<Warning> handler)
    {
        _defaultWarningHandler = handler;
        return this;
    }

    public ResultHandlerBuilder OnDefaultError(Action<Issue> handler)
    {
        _defaultIssueHandler = handler;
        return this;
    }

    public void Execute()
    {
        switch (result)
        {
            case { IsSuccess: true }:
                _successHandler?.Invoke();
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
