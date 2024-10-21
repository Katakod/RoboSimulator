using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using RoboSimulator.Core.Model;

namespace RoboSimulator.ConsoleApp
{
    internal class Program
    {
        private static ILogger<Program>? _logger;
        private const int MAXIMUM_BAD_INPUT_DEFAULT = 3; // Users may try 3 times for each input request 

        private static void Main(string[] args)
        {
            // Set up services and logging
            var serviceProvider = ServiceConfigurator.SetupConfiguration();
            _logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            _logger.LogInformation("RoboSimulator.ConsoleApp start...");
            Console.WriteLine(" ***** Welcome to the Robo Simulator! *****");
            Console.WriteLine("(Press Ctrl+C to exit)");

            try
            {
                Console.CancelKeyPress += new ConsoleCancelEventHandler(OnExit!);

                var room = TryToGetRoomFromInput();
                if (room == null)
                    ExitAppWithError("Simulation failed - Bad input. Room could not be created from input.");

                var robot = TryToGetRobotFromInput(room!);

                var commands = TryToGetCommandsFromInput();
                
                ExecuteSimulation(robot);
                
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _logger.LogInformation("RoboSimulator.ConsoleApp exit...");
            }
        }

        private static Room? TryToGetRoomFromInput()
        {
            Room? room = null;
            int inputCount = 1;

            Console.WriteLine("Enter Room dimensions (width depth) with a space between.");

            while (room == null && inputCount <= MAXIMUM_BAD_INPUT_DEFAULT)
            {
                var input = ReadInput();
                var (dimensions, validationMessage) = InputValidator.ValidateRoomDimensionsInput(input);

                if (dimensions != null)
                {
                    room = new Room(dimensions.Width, dimensions.Depth);
                    _logger!.LogInformation($"Room created with valid input: '{input}'.");
                }
                else
                {
                    Console.WriteLine(validationMessage);
                    _logger!.LogInformation($"Room was not created due to invalid input: '{input}'. {validationMessage}");
                }

                ++inputCount;
            }

            return room;
        }

        private static object TryToGetRobotFromInput(object room)
        {
            throw new NotImplementedException();
        }

        private static object TryToGetCommandsFromInput()
        {
            throw new NotImplementedException();
        }
        private static void ExecuteSimulation(object robot)
        {
            throw new NotImplementedException();
        }

        private static string ReadInput()
        {
            Console.WriteLine("Please enter your input:");
            return Console.ReadLine() ?? "";
        }

        private static void ExitAppWithError(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            _logger!.LogError(errorMessage);
            Environment.Exit(1);
        }

        protected static void OnExit(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
            var message = "RoboSimulator.ConsoleApp exit...User pressed Ctrl+C.";
            Console.WriteLine(message);
            _logger!.LogError(message);
            Environment.Exit(0);
        }
    }
}
