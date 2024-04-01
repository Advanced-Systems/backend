using AdvancedSystems.Backend.Configuration.Settings;
using AdvancedSystems.Backend.Interfaces;
using AdvancedSystems.Backend.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NLog.Extensions.Logging;

namespace AdvancedSystems.Backend
{
    public static class Startup
{
    public static IConfigurationBuilder ConfigureBackendBuilder(this IConfigurationBuilder builder, IHostEnvironment environment)
    {
        return builder.SetBasePath(environment.ContentRootPath)
                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{environment.EnvironmentName}.json",
                                   optional: true,
                                   reloadOnChange: true)
                      .AddEnvironmentVariables();
    }

    public static IServiceCollection ConfigureBackendServices(this IServiceCollection services,
        IHostEnvironment environment,
        IConfigurationRoot configurationRoot)
    {
        services.AddSingleton(environment);

        services.AddOptions<AppSettings>()
                .Bind(configurationRoot.GetRequiredSection(nameof(AppSettings)))
                .ValidateOnStart();

        services.AddLogging(logBuilder =>
        {
            logBuilder.ClearProviders();
            logBuilder.AddNLog();
        });

        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
        });

        services.AddControllers();

        services.AddSingleton<IBookService, BookService>();
        
        return services;
    }

    public static void Configure(this WebApplication app, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
        app.UseAuthorization();
    }
}
}
