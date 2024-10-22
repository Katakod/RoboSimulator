using RoboSimulator.Core.Services;

namespace RoboSimulator.Core.Interfaces;

public interface ICommandService
{
    CommandResultDto ProcessCommands(IRobot robot, string commands);
}