using RoboSimulator.Core.Interfaces;

namespace RoboSimulator.Core.Model;

public class Room : IRoom
{
    public int Width { get; }
    public int Depth { get; }

    public Room(int width, int depth)
    {
        Width = width;
        Depth = depth;
    }

    public bool IsWithinBounds(int x, int y)
    {
        return x >= 1 && x <= Width && y >= 1 && y <= Depth;
    }
}
