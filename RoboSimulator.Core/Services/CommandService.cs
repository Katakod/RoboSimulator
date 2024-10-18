using Microsoft.Extensions.Logging;
using RoboSimulator.Core.Interfaces;

namespace RoboSimulator.Core.Services;

public class CommandService : ICommandService
{
    private readonly ILogger<CommandService> _logger;

    public CommandService(ILogger<CommandService> logger)
    {
        _logger = logger;
    }

    public void ProcessCommands()
    {
        throw new NotImplementedException();
    }
}
