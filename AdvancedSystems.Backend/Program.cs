using AdvancedSystems.Backend.Configuration;

using NLog;
using NLog.Web;

using ILogger = NLog.Logger;

namespace AdvancedSystems.Backend;

public class Program
{
    private static readonly ILogger Logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

    public static void Main(string[] args)
    {
        try
        {
            Logger.Trace("Read configuration file");
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddCustomJsonFile("appsettings.json", true, true)
                .Build();

            var host = CreateHostBuilder(args, configurationRoot);

            Logger.Info("Starting Backend");
            var builder = host.Build();

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
            LogManager.Shutdown();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args, IConfigurationRoot configurationRoot)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder
                    .UseConfiguration(configurationRoot)
                    .UseStartup<Startup>();
            });
    }
}
