using Microsoft.AspNetCore.Mvc;

namespace AdvancedSystems.Backend.Base;

public abstract class BaseController : ControllerBase
{
    public BaseController(ILogger logger)
    {
        this.Logger = logger;
    }

    internal ILogger Logger { get; }
}
