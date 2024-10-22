using RoboSimulator.Core.Model;

namespace RoboSimulator.Core.Services;

public class CommandResultDto
{
    public bool Success { get; set; }
    public string? Message { get; set; } 
    public PositionalDirection? UpdatedPositionalDirection { get; set; }
}
