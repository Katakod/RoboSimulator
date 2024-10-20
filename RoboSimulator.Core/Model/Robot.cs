namespace RoboSimulator.Core.Model;

using RoboSimulator.Core.Interfaces;

public class Robot
{
    public IRoom Room { get; }
    public PositionalDirection PositionalDirection { get; set; }


    public Robot(PositionalDirection positionalDirection, IRoom room)
    {
        PositionalDirection = positionalDirection ?? throw new ArgumentNullException(nameof(positionalDirection));
        Room = room ?? throw new ArgumentNullException(nameof(room)); 

    }

    public void TurnLeft()
    {
        throw new NotImplementedException();
    }

    public void TurnRight()
    {
        throw new NotImplementedException();
    }

    public bool MoveForward()
    {
        throw new NotImplementedException();
    }

}
