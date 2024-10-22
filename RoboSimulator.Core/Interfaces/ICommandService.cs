using RoboSimulator.Core.DTOs;

namespace RoboSimulator.Core.Interfaces;

public interface ICommandService
{
    CommandResultDto ProcessCommands(IRobot robot, string commands);
}