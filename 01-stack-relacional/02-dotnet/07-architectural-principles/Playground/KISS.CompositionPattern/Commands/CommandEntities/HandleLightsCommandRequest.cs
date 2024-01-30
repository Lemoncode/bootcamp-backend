using KISS.Enums;

namespace KISS.Commands.CommandEntities;

internal class HandleLightsCommandRequest
{

    public Light Light { get; set; }

    public bool NewState { get; set; }
}
