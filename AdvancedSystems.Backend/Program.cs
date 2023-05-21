using NLog;
using NLog.Web;

namespace AdvancedSystems.Backend;

public class Program
{
    private static readonly NLog.ILogger Logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

    public static void Main(string[] args)
    {
        try
        {
            var builder = CreateHostBuilder(args).Build();
            Logger.Info("Starting Backend");
            builder.Run();
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "The application was forced to shut down because an unknown error occurred");
            throw;
        }
        finally
        {
            Logger.Info("Shutting down...");
            NLog.LogManager.Shutdown();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseStartup<Startup>();
            });
    }
}
