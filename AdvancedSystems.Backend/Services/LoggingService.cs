using AdvancedSystems.Backend.Models.Interfaces;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace AdvancedSystems.Backend.Service;

public class LoggingService : ILoggingService
{
    public LoggingService(ILogger<LoggingService> logger)
    {
        _logger = logger ?? new NullLogger<LoggingService>();
    }

    private ILogger<LoggingService> _logger;

#pragma warning disable CS8604

    public void LogInformation(string message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }

    public void LogTrace(string message, params object[] args)
    {
        _logger.LogTrace(message, args);
    }

    public void LogDebug(string message, params object[] args)
    {
        _logger.LogDebug(message, args);
    }

    public void LogWarning(string message, params object[] args)
    {
        _logger.LogWarning(message, args);
    }

    public void LogError(string message, params object[] args)
    {
        _logger.LogError(message, args);
    }

    public void LogCritical(string message, params object[] args)
    {
        _logger.LogCritical(message, args);
    }

#pragma warning restore CS8604
}
