using RoboSimulator.Core.Model;

namespace RoboSimulator.Core.Interfaces
{
    public interface IRobot
    {
        PositionalDirection PositionalDirection { get; set; }
        IRoom Room { get; }

        bool MoveForward();
        void TurnLeft();
        void TurnRight();
    }
}