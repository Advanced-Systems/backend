using AdvancedSystems.Backend.Configuration.Settings;
using AdvancedSystems.Backend.Core.Extensions;
using AdvancedSystems.Backend.Interfaces;
using AdvancedSystems.Backend.Services;
using AdvancedSystems.Backend.Services.HealthChecks;

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

        public static IServiceCollection ConfigureBackendServices(this IServiceCollection services, IHostEnvironment environment, IConfigurationRoot configurationRoot)
        {
            services.AddSingleton(environment);

            services.AddOptions<AppSettings>()
                    .Bind(configurationRoot.GetRequiredSection(nameof(AppSettings)))
                    .ValidateOnStart();

            services.AddLogging(options =>
            {
                options.ClearProviders();
                options.AddNLog();

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

            services.AddSingleton<IBookService, BookService>();

            return services;
        }

        #region Custom Service Registration

        public static IServiceCollection AddBackendHealthChecks(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionHealthCheck, ConnectionHealthCheck>();

            services.AddHealthChecks()
                .AddCheck<ConnectionHealthCheck>(nameof(ConnectionHealthCheck));

            return services;
        }

        #endregion

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

            app.MapConnectionHealthCheck(app.Services.GetRequiredService<IConnectionHealthCheck>());

            app.MapControllers();

            app.UseAuthorization();
        }
    }
}
