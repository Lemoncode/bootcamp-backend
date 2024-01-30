using KISS.Agents;
using KISS.Commands.CommandEntities;
using KISS.Entities;

namespace KISS.Commands;

internal class HandleLightsCommand : HomeAutomationCommandBase<HandleLightsCommandRequest, HandleLightsCommandResponse>
{
    public HandleLightsCommand(HandleLightsCommandRequest request, User user, HomeAutomationAgent homeAutomationAgent) : base(request, user, homeAutomationAgent)
    {
    }

    public override void Validate()
    {
        if (this.HomeAutomationAgent.GetState(this.Request.Light) == this.Request.NewState)
        {
            throw new InvalidOperationException("La luz ya tiene el estado deseado.");
        }
        
        base.Validate();
    }

    protected override HandleLightsCommandResponse InnerExecute()
    {
        if (this.Request.NewState)
        {
            this.HomeAutomationAgent.TurnLightOn(this.Request.Light);
        }
        else
        {
            this.HomeAutomationAgent.TurnLightOff(this.Request.Light);
        }
        return new HandleLightsCommandResponse { State = this.Request.NewState };
    }
}
