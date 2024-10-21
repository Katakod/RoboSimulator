using Microsoft.Extensions.Logging;
using RoboSimulator.Core.Interfaces;
using RoboSimulator.Core.Model;

namespace RoboSimulator.Core.Services;

public class CommandService : ICommandService
{
    public IRobot Robot { get; }

    private readonly ILogger<CommandService> _logger;

    public CommandService(IRobot robot, ILogger<CommandService> logger)
    {
        Robot = robot ?? throw new ArgumentNullException(nameof(robot));
        _logger = logger;
    }

    public (bool success, string resultMessage) ProcessCommands(string commands)
    {
        try
        {
            commands = (commands ?? "").Replace(" ", ""); //Allow spaces in command sequence for a more friendly usage
            ValidateCommands(commands);

            foreach (char command in commands)
            {
                switch (command)
                {
                    case 'F':
                        if (!Robot.MoveForward())
                            return ReportOutOfBoundsResult(Robot.PositionalDirection);
                        break;                        
                    case 'L':
                        Robot.TurnLeft();
                        break;
                    case 'R':
                        Robot.TurnRight();
                        break;
                }
            }

            return ReportMoveResult(Robot.PositionalDirection);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;  //TODO: We might wanna return failure instead of throwing exceptions, or treat ValidateCommands different from unexpected errors..
        }
    }

    public static void ValidateCommands(string commands)
    {        
        if (string.IsNullOrEmpty(commands))
            throw new ArgumentNullException(nameof(commands), $"Invalid input - no commands found.");
        if (!commands.All("FLR".Contains))
            throw new ArgumentException($"Invalid input - only F, L and R is allowed in commands sequence.", nameof(commands));
    }
    
    private (bool, string) ReportOutOfBoundsResult(PositionalDirection positionalDirection)
    {
        var message = $"Out of bounds at {positionalDirection.ToPositionString()}.";        
        _logger.LogInformation("{Message}", message);
        return (false, message);
    }

    private (bool, string) ReportMoveResult(PositionalDirection positionalDirection)
    {
        var message = $"Report: {positionalDirection}.";
        _logger.LogInformation("{Message}", message);
        return (true, message);
    }
}
