using Microsoft.Extensions.Logging;
using RoboSimulator.Core.Interfaces;
using RoboSimulator.Core.Model;

namespace RoboSimulator.Core.Services;

public class CommandService : ICommandService
{
    private readonly ILogger<CommandService> _logger;

    public CommandService(ILogger<CommandService> logger, Robot robot)
    {
        _logger = logger;
    }


    public void ProcessCommands(string commands)
    {
        throw new NotImplementedException();
    }
}
