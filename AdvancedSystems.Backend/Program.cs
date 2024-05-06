using System;
using System.IO;

using NLog;
using NLog.Extensions.Logging;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;

namespace AdvancedSystems.Backend;

public static class Program
{
    public static void Main(string[] args)
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        var environment = new HostingEnvironment
        {
            ApplicationName = "AdvancedSystems.Backend",
            ContentRootPath = Directory.GetCurrentDirectory(),
            EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"
        };

        try
        {
            var appBuilder = WebApplication.CreateBuilder(args);

            var configurationRoot = new ConfigurationBuilder()
                                    .ConfigureBackendBuilder(environment)
                                    .Build();

            appBuilder.Configuration.AddConfiguration(configurationRoot);
            appBuilder.Services.ConfigureBackendServices(environment, appBuilder.Configuration);

            LogManager.Configuration = new NLogLoggingConfiguration(appBuilder.Configuration.GetRequiredSection(nameof(NLog)));

            logger.Debug("Starting Backend");
            logger.Debug("Configured Environment: {}", environment.EnvironmentName);

            var backend = appBuilder.Build();
            backend.Configure(environment);
            backend.Run();
        }
        catch (Exception exception)
        {
            logger.Error(exception);
        }
        finally
        {
            LogManager.Shutdown();
            Environment.Exit(0);
        }
    }
}