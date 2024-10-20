namespace RoboSimulator.Core.Interfaces
{
    public interface IRoom
    {
        int Depth { get; }
        int Width { get; }

        bool IsWithinBounds(int x, int y);
    }
}