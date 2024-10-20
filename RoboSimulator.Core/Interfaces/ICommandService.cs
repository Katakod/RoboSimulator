namespace RoboSimulator.Core.Interfaces;

public interface ICommandService
{
    IRobot Robot { get; }

    (bool success, string resultMessage) ProcessCommands(string commands);
}