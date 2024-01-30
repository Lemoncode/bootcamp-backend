// See https://aka.ms/new-console-template for more information
using KISS.Agents;
using KISS.Commands;
using KISS.Commands.CommandEntities;
using KISS.CompositePattern.Validators;
using KISS.Entities;
using KISS.Enums;
using KISS.Validators;

var lightAndDescriptions = new Dictionary<Light, string>
{
    [Light.Hall] = "entrada",
    [Light.HallWay] = "pasillo",
    [Light.BathRoom] = "baño",
    [Light.BedRoom1] = "dormitorio 1",
    [Light.BedRoom2] = "dormitorio 2",
    [Light.DiningRoom] = "comedor",
    [Light.LivingRoom] = "Salón",
    [Light.Office] = "oficina",
};

var homeAutomationAGent = new HomeAutomationAgent { IsConnected = true };

while (true)
{
    Console.WriteLine("¿Qué luz deseas manejar?" + Environment.NewLine +
        string.Join(", ", lightAndDescriptions.Values) + " o salir para salir del programa.");

    var choice = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(choice))
    {
        Console.WriteLine("Debes elegir una opción.");
        continue;
    }

    if (choice == "salir")
    {
        break;
    }

    if (!lightAndDescriptions.ContainsValue(choice))
    {
        Console.WriteLine("Esa luz no se encuentra.");
        continue;
    }

    var light = lightAndDescriptions.Single(kvp => kvp.Value == choice).Key;
    Console.WriteLine("¿Qué deseas hacer?" + Environment.NewLine +
        "1 encender, 2 apagar.");
    var newState = Console.ReadLine();
    if (!int.TryParse(newState, out var iNewState))
    {
        Console.WriteLine($"Error, {newState} no es un valor válido.");
        continue;
    }

    try
    {
        var handleLightsCommand = new HandleLightsCommand(
        new HandleLightsCommandRequest { Light = light, NewState = iNewState == 1 },
        new User { Id = 1, Email = "juanjo@jmontiel.es", Name = "Juanjo", UserName = "jmontiel", IsAdmin = true },
        homeAutomationAGent,
        new HomeAutomationAgentValidator(),
        new AdminUserValidator()
        );
        var response = handleLightsCommand.Execute();
        Console.WriteLine(
             $"El estado de la luz {lightAndDescriptions[light]} es " +
            (response.State ? "encendida" : "apagada") + ".");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}