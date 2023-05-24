using AdvancedSystems.Backend.Models.Core;
using AdvancedSystems.Backend.Configuration;

using Microsoft.OpenApi.Models;

using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

namespace AdvancedSystems.Backend;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        SwaggerSettings = new SwaggerSettings();
        Configuration.GetSection(nameof(SwaggerSettings)).Bind(SwaggerSettings);
    }

    public IConfiguration Configuration { get; }

    public SwaggerSettings SwaggerSettings { get; }

    private static readonly NLog.ILogger Logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

    public void ConfigureServices(IServiceCollection services)
    {
        Logger.Debug("Starting backend service configuration");

        Logger.Trace("Add logging service");
        services.AddLogging(service =>
        {
            service.ClearProviders();
            service.AddNLog();
        });

        Logger.Trace("Add health checks");
        services.AddHealthChecks();

        Logger.Trace("Initiating AppSettings");
        services.AddOptions();
        IConfigurationSection appSettingsSection = this.Configuration.GetSection("AppSettings");
        services.Configure<AppSettings>(appSettingsSection);

        Logger.Trace("Add controllers");
        services.AddControllers();

        Logger.Trace("Add API explorer endpoint");
        services.AddEndpointsApiExplorer();

        Logger.Trace("Add swagger service generation");
        services.AddSwaggerGen(gen  => {
            gen.SwaggerDoc("v1", new OpenApiInfo {
                Title = SwaggerSettings.Title,
                Version = SwaggerSettings.Version,
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        Logger.Debug("Starting backend configuration");

        if (env.IsDevelopment())
        {
            Logger.Trace("Turning on swagger");
            app.UseSwagger(option => {
                option.RouteTemplate = SwaggerSettings.JsonRoute;
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(SwaggerSettings.UiEndpoint, SwaggerSettings.Version);
                options.RoutePrefix = string.Empty;
            });

            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        Logger.Trace("Use health checks");
        app.UseHealthChecks("/health");

        Logger.Trace("Enabling HTTPS");
        app.UseHttpsRedirection();

        Logger.Trace("Enabling static file serving");
        app.UseStaticFiles();

        Logger.Trace("Configuring routing");
        app.UseRouting();

        Logger.Trace("Configuring authorization");
        app.UseAuthorization();

        Logger.Trace("Configuring endpoints");
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
