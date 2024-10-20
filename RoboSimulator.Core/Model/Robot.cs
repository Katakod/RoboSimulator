namespace RoboSimulator.Core.Model;

using RoboSimulator.Core.Interfaces;

public class Robot
{
    public IRoom Room { get; }
    public PositionalDirection PositionalDirection { get; set; }

    private static readonly Direction[] _allDirections = (Direction[])Enum.GetValues(typeof(Direction));


    public Robot(PositionalDirection positionalDirection, IRoom room)
    {
        PositionalDirection = positionalDirection ?? throw new ArgumentNullException(nameof(positionalDirection));
        Room = room ?? throw new ArgumentNullException(nameof(room));

        var x = PositionalDirection.X;
        var y = PositionalDirection.Y;

        if (!Room.IsWithinBounds(x, y))
        {
            var dim = Room.Dimensions;
            throw new ArgumentOutOfRangeException(nameof(positionalDirection),
                $"Initial position ({x},{y}) is out of bounds for the Room ({dim.Width},{dim.Depth}).");
        }
    }

    public void TurnLeft()
    {
        int currentIndex = Array.IndexOf(_allDirections, PositionalDirection.Direction);
        PositionalDirection.Direction = _allDirections[(currentIndex + 3) % 4];  // Left turn (counter-clockwise)
    }

    public void TurnRight()
    {
        int currentIndex = Array.IndexOf(_allDirections, PositionalDirection.Direction);
        PositionalDirection.Direction = _allDirections[(currentIndex + 1) % 4]; // Right turn (clockwise)
    }

    public bool MoveForward()
    {       
        switch (PositionalDirection.Direction)
        {
            case Direction.N: PositionalDirection.Y--; break;
            case Direction.E: PositionalDirection.X++; break;
            case Direction.S: PositionalDirection.Y++; break;
            case Direction.W: PositionalDirection.X--; break;
        }

        if (!Room.IsWithinBounds(PositionalDirection.X, PositionalDirection.Y))
            return true;
        else
            return false;
    }

}
