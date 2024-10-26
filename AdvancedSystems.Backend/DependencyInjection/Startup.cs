using System.IO;

using AdvancedSystems.Backend.Abstractions.Interfaces;

using Asp.Versioning.ApiExplorer;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NLog.Extensions.Logging;

namespace AdvancedSystems.Backend.DependencyInjection;

/// <summary>
///     Initializes services and middleware used by the web application.
/// </summary>
public static class Startup
{
    /// <summary>
    ///     <inheritdoc cref="IStartup.ConfigureServices(IServiceCollection)" />
    /// </summary>
    /// <param name="builder">
    ///     A builder for web applications and services.
    /// </param>
    /// <returns>
    ///     The web application builder.
    /// </returns>
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        var environment = builder.Environment;
        var host = builder.Host;

        host.UseDefaultServiceProvider((context, options) =>
        {
            if (context.HostingEnvironment.IsDevelopment())
            {
                options.ValidateScopes = true;
                options.ValidateOnBuild = true;
            }
            else
            {
                // Disable validation in production for performance reasons
                options.ValidateScopes = false;
                options.ValidateOnBuild = false;
            }
        });

        configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables(); ;

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

        return builder;
    }

    /// <summary>
    ///     <inheritdoc cref="IStartup.Configure(IApplicationBuilder)" />
    /// </summary>
    /// <param name="app">
    ///     <inheritdoc cref="WebApplication"/>
    /// </param>
    /// <returns>
    ///     Returns the web application.
    /// </returns>
    public static WebApplication Configure(this WebApplication app)
    {
        app.UseStatusCodePages();
        app.UseExceptionHandler();

        if (app.Environment.IsProduction())
        {
            app.UseHsts();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                foreach (ApiVersionDescription description in app.DescribeApiVersions())
                {
                    option.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
                }
            });
        }

        app.UseHttpsRedirection();
        app.UseStatusCodePages();
        app.UseExceptionHandler();
        app.UseRouting();

        app.AddHealthCheck<IConnectionHealthCheck>();

        app.MapControllers();
        app.UseAuthorization();

        return app;
    }
}