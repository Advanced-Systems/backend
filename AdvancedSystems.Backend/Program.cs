using System.Threading.Tasks;

using AdvancedSystems.Backend.DependencyInjection;

using Microsoft.AspNetCore.Builder;

namespace AdvancedSystems.Backend;

public static class Program
{
    /// <summary>
    ///     Defines the entry point of the application.
    /// </summary>
    /// <param name="args">
    ///     Optional command line arguments.
    /// </param>
    /// <returns>
    ///     An asynchronous task to wait.
    /// </returns>
    public static async Task Main(string[] args)
    {
        await WebApplication.CreateBuilder(args)
            .ConfigureServices()
            .Build()
            .Configure()
            .RunAsync();
    }
}