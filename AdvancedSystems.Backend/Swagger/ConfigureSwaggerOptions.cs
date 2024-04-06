using Asp.Versioning.ApiExplorer;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace AdvancedSystems.Backend.Swagger
{
    public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IHostEnvironment environment) : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider = provider;
        private readonly IHostEnvironment _environment = environment;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (ApiVersionDescription description in this._provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    new OpenApiInfo
                    {
                        Title = this._environment.ApplicationName,
                        Version = description.ApiVersion.ToString(),
                    }
                );
            }
        }
    }
}
