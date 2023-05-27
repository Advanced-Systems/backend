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
        Logger.Debug("Start service configuration");

        Logger.Trace("Add logging service");
        services.AddLogging(service =>
        {
            service.ClearProviders();
            service.AddNLog();
        });

        Logger.Trace("Add health checks");
        services.AddHealthChecks();

        Logger.Trace("Add API versioning");
        services.AddApiVersioning(options => {
            options.DefaultApiVersion = new ApiVersion(AppSettings.DefaultApiVersion);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = new HeaderApiVersionReader("api-version");
        });

        Logger.Trace("Initialize settings");
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
        Logger.Debug("Start builder configuration");

        if (env.IsDevelopment())
        {
            Logger.Trace("Turn on swagger");
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
        app.UseEndpoints(endpoints =>
        {
            Logger.Trace("Configure API versioning");
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
