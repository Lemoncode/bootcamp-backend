using KISS.Commands.CommandEntities;

namespace KISS.Commands;

internal abstract class CommandBase<TRequest, TResponse>
    where TRequest : CommandRequestBase
    where TResponse : CommandResponseBase, new()
{
    private readonly TRequest _request;

    public CommandBase(TRequest request)
    {
        _request = request;
    }

    protected TRequest Request => _request;
    
    protected virtual TResponse InnerExecute()
    {
        return new();
    }

    public virtual void Validate()
    {
    }

    public TResponse Execute()
    {
        Validate();
        return InnerExecute();
    }
}
