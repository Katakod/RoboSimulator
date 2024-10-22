using Microsoft.Extensions.Logging;
using RoboSimulator.Core.Interfaces;

namespace RoboSimulator.Core.Infrastructure;

public class SimpleExceptionHandler : IExceptionHandler
{
    private readonly ILogger<SimpleExceptionHandler> _logger;

    public SimpleExceptionHandler(ILogger<SimpleExceptionHandler> logger)
    {
        _logger = logger;
    }

    public void Handle(Exception ex)
    {
        var message = $"SimpleExceptionHandler caught an unhandled error. {ex.Message}";
        _logger.LogError(ex, "{Message}", message);
        throw ex;
    }
}

