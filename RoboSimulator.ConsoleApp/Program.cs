using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RoboSimulator.Core.DTOs;
using RoboSimulator.Core.Interfaces;
using RoboSimulator.Core.Model;
using System.Text;

namespace RoboSimulator.ConsoleApp
{
    internal class Program
    {
        private static ServiceProvider? _serviceProvider;
        private static ILogger<Program>? _logger;
        private const int MAXIMUM_BAD_INPUT_DEFAULT = 3; // Users may try 3 times for each input request 

        private static void Main()
        {
            try
            {
                // Set up services and logging
                _serviceProvider = ServiceConfigurator.SetupConfiguration();
                _logger = _serviceProvider.GetRequiredService<ILogger<Program>>();

                LogInfo("RoboSimulator.ConsoleApp start...");
                Console.WriteLine(" ***** Welcome to the Robo Simulator! (Press Ctrl+C to exit) *****");

                Console.CancelKeyPress += new ConsoleCancelEventHandler(OnExit!);

                var room = TryToGetRoomFromInput();
                if (room == null)
                    ExitAppWithError("ERROR: Bad input. Room could not be created from input.");

                var robot = TryToGetRobotFromInput(room!);
                if (robot == null)
                    ExitAppWithError("ERROR: Bad input. Robot could not be created from input.");


                Console.WriteLine("\nEnter Robot commands to move it inside the room. For example: 'F LF R FF' \n" +
                                    "F=Forward, L=Turn Left, R=Turn Right. Spaces will be ignored.");
                while (true)
                {
                    var commands = TryToGetCommandsFromInput();
                    if (commands == null)
                        ExitAppWithError("ERROR: Bad input. No valid sequence of movement commands in input.");

                    var result = ExecuteRobotSimulation(robot!, commands!);

                    HandleSimulationResult(result);
                }
            }
            catch (Exception ex)
            {
                ExitAppWithError($"ERROR: Simulation failed due to an unexpected error: {ex.Message}", ex);
            }
            finally
            {
                LogInfo("RoboSimulator.ConsoleApp exit...");
            }
        }

        private static Room? TryToGetRoomFromInput()
        {
            Room? room = null;
            int inputCount = 1;

            Console.WriteLine("\nEnter Room dimensions (width depth) with a space between.");

            while (room == null && inputCount <= MAXIMUM_BAD_INPUT_DEFAULT)
            {
                var input = ReadInput();
                var (dimensions, validationMessage) = InputValidator.ValidateRoomDimensionsInput(input);

                if (dimensions != null)
                {
                    room = new Room(dimensions.Width, dimensions.Depth);
                    LogInfo($"Room created with valid input: '{input}'.");
                }
                else
                {
                    Console.WriteLine(validationMessage);
                    LogInfo($"Room was not created due to invalid input: '{input}'. {validationMessage}");
                }

                ++inputCount;
            }

            return room;
        }

        private static IRobot? TryToGetRobotFromInput(Room room)
        {
            IRobot? robot = null;
            int inputCount = 1;

            Console.WriteLine("\nEnter Robot start values (x y direction) with spaces between.");

            while (robot == null && inputCount <= MAXIMUM_BAD_INPUT_DEFAULT)
            {
                var input = ReadInput();
                var (startPosition, validationMessage) = InputValidator.ValidateRobotPositionInput(input, room);
                if (startPosition != null)
                {
                    robot = new Robot(startPosition, room);
                    LogInfo($"Robot created with valid input: '{input}'.");
                }
                else
                {
                    Console.WriteLine(validationMessage);
                    LogInfo($"No Robot was created due to invalid input: '{input}'. {validationMessage}");
                }

                ++inputCount;
            }

            return robot;
        }

        private static string TryToGetCommandsFromInput()
        {
            string validInput = null!;
            int inputCount = 1;

            while (validInput == null && inputCount <= MAXIMUM_BAD_INPUT_DEFAULT)
            {
                var input = ReadInput();
                var (isValid, resultMessage) = InputValidator.ValidateCommandsInput(input);

                if (isValid)
                {
                    validInput = input;
                    LogInfo($"Command input '{input}' is valid.\n {resultMessage}");
                }
                else
                {
                    Console.WriteLine(resultMessage);
                    LogInfo($"Invalid command input '{input}' entered.{resultMessage}");
                }

                ++inputCount;
            }

            return validInput!;
        }

        private static CommandResultDto ExecuteRobotSimulation(IRobot robot, string commands)
        {
            var commandService = _serviceProvider!.GetRequiredService<ICommandService>();

            return commandService.ProcessCommands(robot, commands);
        }


        private static void HandleSimulationResult(CommandResultDto result)
        {
            if (result.Success)
            {
                Console.WriteLine(result.Message);
            }
            else
            {
                ExitAppWithError($"ERROR: {result.Message}");
            }
        }

        private static string ReadInput()
        {
            var inputBuilder = new StringBuilder();

            Console.WriteLine("Please enter your input:");

            // CancelKeyPress event had race condition problems with simple Console.ReadKey(),
            // so we're walking through read input more granular, inspecting each key press
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey(intercept: true);

                    if ((keyInfo.Modifiers == ConsoleModifiers.Control && keyInfo.Key == ConsoleKey.C) ||
                        keyInfo.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                        break;
                    }
                    else if (keyInfo.Key == ConsoleKey.Backspace)
                    {
                        if (inputBuilder.Length > 0)
                        {
                            inputBuilder.Length--;

                            // Move the cursor back, space overwrite and move back again
                            Console.Write("\b \b");
                        }
                    }
                    else if (char.IsLetterOrDigit(keyInfo.KeyChar) || keyInfo.Key == ConsoleKey.Spacebar)
                    {
                        inputBuilder.Append(keyInfo.KeyChar);
                        Console.Write(keyInfo.KeyChar); // Echo the character
                    }
                }
            }
            return inputBuilder.ToString();
        }

        private static void LogInfo(string message)
        {
            _logger!.LogInformation("{Message}", message);
        }

        private static void ExitApp(string message)
        {
            var exitCode = 0;
            var exitMessage = $"Exit Code: {exitCode}.";

            Console.WriteLine($"\n{message}\n{exitMessage}");

            _logger!.LogInformation("{Message}", message);
            _logger!.LogInformation("{Message}", exitMessage);

            Environment.Exit(exitCode);
        }

        private static void ExitAppWithError(string errorMessage, Exception? ex = null)
        {
            int exitCode = ex != null ? 1 : 2;
            var exitMessage = $"Exit Code: {exitCode}. ";

            Console.WriteLine($"\n{errorMessage}\n{exitMessage}");

            _logger!.LogError(ex, "{Message}", errorMessage);
            _logger!.LogInformation("{Message}", exitMessage);

            Environment.Exit(exitCode);
        }


        protected static void OnExit(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
            ExitApp("User pressed Ctrl+C to exit.");

        }
    }
}
