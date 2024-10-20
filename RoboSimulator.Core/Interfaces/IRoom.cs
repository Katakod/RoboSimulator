using RoboSimulator.Core.Model;

namespace RoboSimulator.Core.Interfaces;

public interface IRoom
{
    public Dimensions Dimensions { get; }

    bool IsWithinBounds(int x, int y);

}