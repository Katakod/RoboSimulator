using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace RoboSimulator.ConsoleApp
{
    internal class Program
    {
        private static ILogger<Program>? _logger;

        private static void Main(string[] args)
        {
            // Set up services and logging           
            var serviceProvider = 
                new ServiceCollection().AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddNLog();
                })
                .BuildServiceProvider();
            
            _logger = serviceProvider.GetRequiredService<ILogger<Program>>();


            _logger.LogInformation("RoboSimulator.ConsoleApp start...");
            Console.WriteLine(" ***** Welcome to the Robo Simulator! *****");
            Console.WriteLine("Press Ctrl+C to exit.");

            try
            {
                Console.CancelKeyPress += new ConsoleCancelEventHandler(OnExit!);

                var room = TryToGetRoomFromInput();
       
                var robot = TryToGetRobotFromInput(room);

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

        private static object TryToGetRoomFromInput()
        {
            var input = Console.ReadKey();
            InputValidator.ValidateRoomInput(input);

            throw new NotImplementedException();
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
