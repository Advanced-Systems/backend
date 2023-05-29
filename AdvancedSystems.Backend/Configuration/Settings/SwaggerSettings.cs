using Microsoft.OpenApi.Models;

namespace AdvancedSystems.Backend.Configuration.Settings;

public class SwaggerSettings
{
    public string Title { get; set; } = "ASP.NET Core REST API";

    public OpenApiContact? Contact { get; set; }

    public OpenApiLicense? License { get; set; }
}
