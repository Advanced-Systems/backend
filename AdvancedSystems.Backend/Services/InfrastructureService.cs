using AdvancedSystems.Backend.Interfaces;

using Asp.Versioning;

using Microsoft.AspNetCore.Http;

namespace AdvancedSystems.Backend.Services;

public sealed class InfrastructureService : IInfrastructureService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public InfrastructureService(IHttpContextAccessor httpContextAccessor)
    {
        this._httpContextAccessor = httpContextAccessor;
    }

    #region Properties

    public ApiVersion? RequestedApiVersion => this._httpContextAccessor.HttpContext?.GetRequestedApiVersion();

    #endregion
}
