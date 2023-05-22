using AdvancedSystems.Backend.Models.Core;

using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

namespace AdvancedSystems.Backend;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

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

        Logger.Trace("Initiating AppSettings");
        services.AddOptions();
        IConfigurationSection appSettingsSection = this.Configuration.GetSection("AppSettings");
        services.Configure<AppSettings>(appSettingsSection);

        Logger.Trace("Add controllers");
        services.AddControllers();

        Logger.Trace("Add API explorer endpoint");
        services.AddEndpointsApiExplorer();

        Logger.Trace("Add swagger service generation");
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        Logger.Debug("Starting backend configuration");

        if (env.IsDevelopment())
        {
            Logger.Trace("Turning on swagger");
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }

        Logger.Trace("Enabling HTTPS");
        app.UseHsts();
        app.UseHttpsRedirection();

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
