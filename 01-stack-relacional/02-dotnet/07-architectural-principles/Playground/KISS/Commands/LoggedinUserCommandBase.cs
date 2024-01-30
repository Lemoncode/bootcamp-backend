using KISS.Commands.CommandEntities;
using KISS.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISS.Commands;

internal class LoggedinUserCommandBase<TRequest, TResponse> : CommandBase<TRequest, TResponse>
    where TRequest : CommandRequestBase
    where TResponse : CommandResponseBase, new()
{

    private readonly User _user;

    protected User User => _user;

    public LoggedinUserCommandBase(TRequest request, User user) : base(request)
    {
        _user = user;
    }

    public override void Validate()
    {
        if (_user is null)
        {
            throw new Exception("Debes iniciar sesión.");
        }
        
        base.Validate();
    }
}
