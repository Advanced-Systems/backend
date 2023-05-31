namespace AdvancedSystems.Backend.Models.Interfaces;

public interface ILoggingService
{
    public void LogInformation(string message, params object[] args);

    public void LogTrace(string message, params object[] args);

    public void LogDebug(string message, params object[] args);

    public void LogWarning(string message, params object[] args);

    public void LogError(string message, params object[] args);

    public void LogCritical(string message, params object[] args);
}
