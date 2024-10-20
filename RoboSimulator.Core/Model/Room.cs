using RoboSimulator.Core.Interfaces;

namespace RoboSimulator.Core.Model;

public class Room : IRoom
{
    public Dimensions Dimensions { get; }

    public Room(int width, int depth)
    {
        if (width < 1)
            throw new ArgumentOutOfRangeException(nameof(width), "Width of a Room must be greater than 0.");
        if (depth < 1)
            throw new ArgumentOutOfRangeException(nameof(depth), "Depth of a Room must be greater than 0.");

        Dimensions = new Dimensions(width, depth);
    }

    public bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < Dimensions.Width && y >= 0 && y < Dimensions.Depth;
    }
}
