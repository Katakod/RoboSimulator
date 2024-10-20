using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using RoboSimulator.Core.Interfaces;
using RoboSimulator.Core.Model;
using RoboSimulator.Core.Services;

namespace RoboSimulator.ConsoleApp
{
    public static class ServiceConfigurator
    {
        public static ServiceProvider SetupConfiguration()
        {
            return new ServiceCollection()
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddNLog();
                })
                .AddSingleton<ICommandService, CommandService>()
                .AddSingleton<IRobot, Robot>()
                //.AddSingleton<InputValidator>()
                .BuildServiceProvider();
        }
    }
}
