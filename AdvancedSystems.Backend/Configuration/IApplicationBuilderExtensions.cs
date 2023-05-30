using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

namespace AdvancedSystems.Backend.Configuration;

public static class IApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
    {
        return app.UseSwagger(option =>
        {
            option.RouteTemplate = "swagger/{documentName}/swagger.json";
        });
    }

    public static IApplicationBuilder UseCustomSwaggerUI(this IApplicationBuilder app)
    {
        return app.UseSwaggerUI(options =>
        {
            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                string name = description.GroupName;
                string url = $"swagger/{name}/swagger.json";
                options.SwaggerEndpoint(url, name);
            }

            options.RoutePrefix = string.Empty;
        });
    }

    public static IApplicationBuilder UseCustomEndpoints(this IApplicationBuilder app, int defaultApiVersion)
    {
        return app.UseEndpoints(endpoints =>
        {
            var apiVersionSet = endpoints.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(defaultApiVersion))
                .ReportApiVersions()
                .Build();

            endpoints.MapControllers()
                .WithApiVersionSet(apiVersionSet)
                .MapToApiVersion(defaultApiVersion);
        });
    }
}
