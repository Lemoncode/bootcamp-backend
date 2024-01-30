using KISS.Agents;
using KISS.Commands.CommandEntities;
using KISS.Contracts;
using KISS.Entities;

namespace KISS.Commands;

internal class HandleLightsCommand
{
    private readonly HomeAutomationAgent _agent;

    private readonly User _user;

    private readonly IValidator<HomeAutomationAgent> _agentValidator;

    private readonly IValidator<User> _userValidator;
    
    private readonly HandleLightsCommandRequest _request;

    public HandleLightsCommand(HandleLightsCommandRequest request, User user, HomeAutomationAgent homeAutomationAgent, IValidator<HomeAutomationAgent> validatorAGent, IValidator<User> userValidator)
    {
        _request = request;
        _user = user;
        _agent = homeAutomationAgent;
        _agentValidator = validatorAGent;
        _userValidator = userValidator;
    }

    public void Validate()
    {
        _agentValidator.Validate(_agent);
        _userValidator.Validate(_user);
        if (_agent.GetState(_request.Light) == _request.NewState)
        {
            throw new InvalidOperationException("La luz ya tiene el estado deseado.");
        }
    }

    public HandleLightsCommandResponse Execute()
    {
        if (_request.NewState)
        {
            _agent.TurnLightOn(_request.Light);
        }
        else
        {
            _agent.TurnLightOff(_request.Light);
        }
        return new HandleLightsCommandResponse { State = _request.NewState };
    }
}
