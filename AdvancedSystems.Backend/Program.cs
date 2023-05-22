using AdvancedSystems.Backend.Extensions;

using NLog;
using NLog.Web;

namespace AdvancedSystems.Backend;

public class Program
{
    private static readonly NLog.ILogger Logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

    public static void Main(string[] args)
    {
        try
        {
            Logger.Trace("Reading configuration file");
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
            NLog.LogManager.Shutdown();
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
