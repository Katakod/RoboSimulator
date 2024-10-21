
using RoboSimulator.Core.Interfaces;
using RoboSimulator.Core.Model;

namespace RoboSimulator.ConsoleApp
{
    public class InputValidator
    {
        public static (Dimensions? dimensions, string resultMessage) ValidateRoomDimensionsInput(string input)
        {
            var parts = (input ?? "").TrimEnd().Split(' ');

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

            return (null, "Invalid Room dimensions. Enter valid numbers for width and depth.");
        }

        public static (PositionalDirection? startPosition, string resultMessage) ValidateRobotPositionInput(string input, Room room)
        {
            ArgumentNullException.ThrowIfNull(room, nameof(room));

            var parts = (input ?? "").TrimEnd().Split(' ');
            if (parts.Length == 3 &&
                int.TryParse(parts[0], out int x) &&
                int.TryParse(parts[1], out int y) &&
                TryParseDirection(parts[2], out Direction direction))
            {
                var startPosition = new PositionalDirection(x, y, direction);

                if (room.IsWithinBounds(startPosition.X, startPosition.Y))
                {
                    return (startPosition, $"Valid Robot position entered for Room ({room.Dimensions}): {startPosition}.");
                }
                else
                {
                    return (null, $"Invalid robot starting position. Position ({startPosition.ToPositionString()}) is out of bounds for this {room.Dimensions} Room.");
                }
            }

            return (null, "Invalid robot starting position. Enter valid number for x,y and a direction (N, E, S, W).");
        }

        public static bool TryParseDirection(string input, out Direction direction)
        {
            input = (input ?? "").ToUpper();

            if (Enum.TryParse(input, out Direction parsedDirection) &&
                Enum.IsDefined(typeof(Direction), parsedDirection))
            {
                direction = parsedDirection;
                return true;
            }

            direction = default;
            return false;
        }

        public static (bool isValid, string resultMessage) ValidateCommandsInput(string input)
        {
            input = (input ?? "").Replace(" ", "");

            if (input.All("FLR".Contains))
            {
                return (true, "Valid commands entered."); ;
            }

            return (false, "Invalid commands entered. Only F, L, R (and space) are allowed.");
        }


    }
}