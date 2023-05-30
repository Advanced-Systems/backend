using AdvancedSystems.Backend.Configuration;
using AdvancedSystems.Backend.Configuration.Settings;

using NLog;
using NLog.Web;
using Microsoft.OpenApi.Models;

using ILogger = NLog.Logger;

namespace AdvancedSystems.Backend;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;

        SwaggerSettings = new SwaggerSettings();
        Configuration.GetSection(nameof(SwaggerSettings)).Bind(SwaggerSettings);

        AppSettings = new AppSettings();
        Configuration.GetSection(nameof(AppSettings)).Bind(AppSettings);

        DefaultApiVersion = $"v{AppSettings.DefaultApiVersion}";
    }

    public IConfiguration Configuration { get; }

    public SwaggerSettings SwaggerSettings { get; }

    public AppSettings AppSettings { get; }

    private string DefaultApiVersion { get; }

    private static readonly ILogger Logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

    public void ConfigureServices(IServiceCollection services)
    {
        Logger.Debug("Start service configuration");
        services.Configure<RouteOptions>(options => {
            options.LowercaseUrls = true;
        });

        Logger.Trace("Add logging service");
        services.AddCustomLogging();

        Logger.Trace("Add health checks");
        services.AddHealthChecks();

        Logger.Trace("Initialize settings");
        services.AddOptions();
        services.Configure<AppSettings>(this.Configuration.GetSection(nameof(AppSettings)));

        Logger.Trace("Add controllers");
        services.AddControllers();

        Logger.Trace("Add API versioning");
        services.AddCustomApiVersioning(AppSettings.DefaultApiVersion);

        Logger.Trace("Add swagger service generation");
        services.AddCustomSwaggerGen(new OpenApiInfo {
            Title = SwaggerSettings.Title,
            Version = DefaultApiVersion,
            Contact = SwaggerSettings.Contact,
            License = SwaggerSettings.License
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        Logger.Debug("Start builder configuration");

        if (env.IsDevelopment())
        {
            Logger.Trace("Turn on swagger");
            app.UseCustomSwagger();
            app.UseCustomSwaggerUI();

            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        Logger.Trace("Add health check");
        app.UseHealthChecks("/health");

        Logger.Trace("Enable HTTPS");
        app.UseHttpsRedirection();

        Logger.Trace("Enable static file serving");
        app.UseStaticFiles();

        Logger.Trace("Configure routing");
        app.UseRouting();

        Logger.Trace("Configure authorization");
        app.UseAuthorization();

        Logger.Trace("Configure endpoints");
        app.UseCustomEndpoints(AppSettings.DefaultApiVersion);
    }
}
