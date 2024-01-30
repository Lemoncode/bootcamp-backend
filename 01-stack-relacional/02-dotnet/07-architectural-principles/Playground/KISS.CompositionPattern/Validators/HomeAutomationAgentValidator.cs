using KISS.Agents;
using KISS.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISS.Validators;

internal class HomeAutomationAgentValidator : IValidator<HomeAutomationAgent>
{
    public void Validate(HomeAutomationAgent agent)
    {
        if (agent    is null || !agent.IsConnected)
        {
            throw new Exception("El agente de manejo de la domótica no está conectado. Conéctalo e inténtalo de nuevo.");
        }
    }
}
