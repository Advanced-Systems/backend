using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdvancedSystems.Backend.Core;

internal class ExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(ILogger<ExceptionHandler> logger)
    {
        this._logger = logger;
    }

    private static (int statusCode, string title) MapException(Exception exception)
    {
        return exception switch
        {
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        (int statusCode, string title) = MapException(exception);

        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        this._logger.LogError(exception, "{Title} (On machine '{MachineName}' with TraceId={TraceId})", title, Environment.MachineName, traceId);

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = statusCode,
            Type = exception.GetType().Name,
            Title = title,
            Detail = exception.Message,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
        }, cancellationToken: cancellationToken);

        return true;
    }
}
