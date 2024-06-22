using System.Threading.Tasks;

using AdvancedSystems.Backend.Core;

namespace AdvancedSystems.Backend;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Startup.ConfigureBuilder(args);
        var configuration = builder.Configuration;
        var environment = builder.Environment;
        
        builder.Services.ConfigureServices(configuration, environment);

        var backend = builder.Build();
        backend.Configure(environment);
        await backend.RunAsync();
    }
}