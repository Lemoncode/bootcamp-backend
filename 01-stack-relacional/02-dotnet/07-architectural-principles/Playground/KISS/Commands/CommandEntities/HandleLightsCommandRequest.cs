using KISS.Enums;

namespace KISS.Commands.CommandEntities;

internal class HandleLightsCommandRequest : CommandRequestBase
{

    public Light Light { get; set; }

    public bool NewState { get; set; }
}
