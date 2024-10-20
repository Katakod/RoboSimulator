﻿namespace RoboSimulator.Core.Model;
public class PositionalDirection
{
    public int X { get; set; }
    public int Y { get; set; }
    public Direction Direction { get; set; }

    public PositionalDirection(int x, int y, Direction direction)
    {
        X = x;
        Y = y;
        Direction = direction;
    }

}