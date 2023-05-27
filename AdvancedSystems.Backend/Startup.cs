using AdvancedSystems.Backend.Models.Core;
using AdvancedSystems.Backend.Configuration;

using Microsoft.OpenApi.Models;

using Asp.Versioning;
using Asp.Versioning.Conventions;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

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
        Logger.Debug("Starting backend service configuration");

        Logger.Trace("Add logging service");
        services.AddLogging(service =>
        {
            service.ClearProviders();
            service.AddNLog();
        });

        Logger.Trace("Add health checks");
        services.AddHealthChecks();

        Logger.Trace("Add API Versioning");
        services.AddApiVersioning(options => {
            options.DefaultApiVersion = new ApiVersion(AppSettings.DefaultApiVersion);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = new HeaderApiVersionReader("api-version");
        });

        Logger.Trace("Initiating Settings");
        services.AddOptions();
        services.Configure<AppSettings>(this.Configuration.GetSection(nameof(AppSettings)));

        Logger.Trace("Add controllers");
        services.AddControllers();

        Logger.Trace("Add API explorer endpoint");
        services.AddEndpointsApiExplorer();

        Logger.Trace("Add swagger service generation");
        services.AddSwaggerGen(gen  => {
            gen.SwaggerDoc(DefaultApiVersion, new OpenApiInfo {
                Title = SwaggerSettings.Title,
                Version = DefaultApiVersion,
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
                option.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(options =>
            {
                string url = $"swagger/{DefaultApiVersion}/swagger.json";
                options.SwaggerEndpoint(url, DefaultApiVersion);
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
            Logger.Trace("Configure API Versioning");
            var apiVersionSet = endpoints.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(AppSettings.DefaultApiVersion))
                .ReportApiVersions()
                .Build();

            endpoints.MapControllers()
                .WithApiVersionSet(apiVersionSet)
                .MapToApiVersion(AppSettings.DefaultApiVersion);
        });
    }
}
