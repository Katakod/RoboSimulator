using Microsoft.Extensions.Logging;
using RoboSimulator.Core.DTOs;
using RoboSimulator.Core.Interfaces;
using RoboSimulator.Core.Model;

namespace RoboSimulator.Core.Services;

public class CommandService : ICommandService
{
    private readonly ILogger<CommandService> _logger;
    private readonly IExceptionHandler _exceptionHandler;

    public CommandService(ILogger<CommandService> logger, IExceptionHandler exceptionHandler)
    {        
        _logger = logger;
        _exceptionHandler = exceptionHandler;
    }

    public CommandResultDto ProcessCommands(IRobot robot, string commands)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(robot, nameof(robot));

            commands = (commands ?? "").Replace(" ", ""); //Allow spaces in command sequence for a more friendly usage
            ValidateCommands(commands);

            foreach (char command in commands)
            {
                switch (command)
                {
                    case 'F':
                        if (!robot.MoveForward())
                            return ReportOutOfBoundsResult(robot.PositionalDirection);
                        break;                        
                    case 'L':
                        robot.TurnLeft();
                        break;
                    case 'R':
                        robot.TurnRight();
                        break;
                }
            }

            return ReportSuccessResult(robot.PositionalDirection);
        }
        catch (Exception ex)
        {            
            _exceptionHandler.Handle(ex);
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
    
    private CommandResultDto ReportOutOfBoundsResult(PositionalDirection positionalDirection)
    {
        var message = $"Out of bounds at {positionalDirection.ToPositionString()}.";        
        _logger.LogInformation("{Message}", message);

        return ReportCommandResult(false, message, positionalDirection);
    }

    private CommandResultDto ReportSuccessResult(PositionalDirection positionalDirection)
    {
        var message = $"Report: {positionalDirection}.";
        return ReportCommandResult(true, message, positionalDirection);
    }

    private CommandResultDto ReportCommandResult(bool success, string message, PositionalDirection positionalDirection)
    {
        _logger.LogInformation("{Message}", message);

        return new CommandResultDto
        {
            Success = success,
            Message = message,
            UpdatedPositionalDirection = positionalDirection
        };
    }
}
