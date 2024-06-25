using System.IO;

using AdvancedSystems.Backend.Extensions;
using AdvancedSystems.Backend.Interfaces;

using Asp.Versioning.ApiExplorer;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NLog.Extensions.Logging;

namespace AdvancedSystems.Backend.Core;

internal static class Startup
{
    internal static WebApplicationBuilder ConfigureBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
        
        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
        builder.Services.AddSingleton<IHostEnvironment>(builder.Environment);

        return builder;
    }

    internal static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        
        services.AddBackendSettings(configuration);

        services.AddLogging(options =>
        {
            options.ClearProviders();
            options.AddNLog(new NLogLoggingConfiguration(configuration.GetRequiredSection("NLog")));

            if (environment.IsDevelopment())
            {
                options.AddConsole();
            }
        });

        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
        });

        services.AddControllers();

        services.AddBackendHealthChecks();
        services.AddBackendDocumentation(configuration);
        services.AddBackendServices(environment);

        return services;
    }

    internal static void Configure(this WebApplication app, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(option => {
                foreach (ApiVersionDescription description in app.DescribeApiVersions())
                {
                    option.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
                }
            });
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStatusCodePages();
        app.UseExceptionHandler();

        app.UseRouting();
        
        app.MapConnectionHealthCheck<IConnectionHealthCheck>();

        app.MapControllers();

        app.UseAuthorization();
    }
}
