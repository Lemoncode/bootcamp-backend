using KISS.Enums;

using System.Collections.Concurrent;

namespace KISS.Agents;

internal class HomeAutomationAgent
{

    public bool IsConnected { get; set; }

    private Dictionary<Light, bool> _lightStates = new Dictionary<Light, bool>();

    public void TurnLightOn(Light light)
    {
        if (_lightStates.TryGetValue(light, out var state) && state)
        {
            throw new InvalidOperationException("La luz ya estaba encendida.");
        }
        _lightStates[light] = true;
    }

    public void TurnLightOff(Light light)
    {
        if (_lightStates.TryGetValue(light, out var state) && !state)
        {
            throw new InvalidOperationException("La luz ya estaba encendida.");
        }

        _lightStates[light] = false;
    }

    public bool GetState(Light light) =>
        _lightStates.TryGetValue(light, out var currentState) ? currentState : false;
}
