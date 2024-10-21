
using RoboSimulator.Core.Model;

namespace RoboSimulator.ConsoleApp
{
    public class InputValidator
    {
        public static (Dimensions? dimensions, string resultMessage) ValidateRoomDimensionsInput(string input)
        {
            var parts = (input ?? "").Split(' ');

            if (parts.Length == 2 && 
                int.TryParse(parts[0], out int width) && 
                int.TryParse(parts[1], out int depth))
            {
                if (width > 0 && depth > 0)
                {
                    var dimensions = new Dimensions(width, depth);
                    return (dimensions, $"Valid Room dimensions entered: {dimensions}.");
                }
            }

            return (null, "Invalid Room dimensions. Enter valid numbers for width and height.");
        }

        public static (PositionalDirection? startPosition, string resultMessage) ValidateRobotInput(string input)
        {
            throw new NotImplementedException();
        }

        public static (bool isValidCommand, string resultMessage) ValidateCommandsInput(string input)
        {
            throw new NotImplementedException();
        }


    }
}