using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdvancedSystems.Backend.Base;

public abstract class BaseController : ControllerBase
{
    public BaseController(ILogger logger)
    {
        this.Logger = logger;
    }

    internal ILogger Logger { get; }
}
