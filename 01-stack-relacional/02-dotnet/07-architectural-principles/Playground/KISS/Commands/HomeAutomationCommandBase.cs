using KISS.Agents;
using KISS.Commands.CommandEntities;
using KISS.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISS.Commands;

internal class HomeAutomationCommandBase<TRequest, TResponse> : AdminCommandBase<TRequest, TResponse>
    where TRequest : CommandRequestBase
    where TResponse : CommandResponseBase, new()
{
    private readonly HomeAutomationAgent _homeAutomationAgent;

    protected HomeAutomationAgent HomeAutomationAgent => _homeAutomationAgent;

    public HomeAutomationCommandBase(TRequest request, User user, HomeAutomationAgent homeAutomationAgent) : base(request, user)
    {
        _homeAutomationAgent = homeAutomationAgent;
    }


    public override void Validate()
    {
        if (HomeAutomationAgent is null || !HomeAutomationAgent.IsConnected)
        {
            throw new Exception("El agente de manejo de la domótica no está conectado. Conéctalo e inténtalo de nuevo.");
        }

        base.Validate();
    }

}
