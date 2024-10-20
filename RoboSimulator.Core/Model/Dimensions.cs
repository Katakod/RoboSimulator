namespace RoboSimulator.Core.Model;

public class Dimensions
{
    public int Width { get; }
    public int Depth { get; }

    public Dimensions(int width, int depth)
    {
        Width = width;
        Depth = depth;
    }

    public override string ToString()
    {
        return $"{Width} x {Depth}";
    }

}
