using AdvancedSystems.Backend.Interfaces;
using AdvancedSystems.Backend.Services;

using Microsoft.AspNetCore.Http;

using Moq;

namespace AdvancedSystems.Backend.Tests.Fixtures;

public sealed class InfrastructureServiceFixture
{
    public InfrastructureServiceFixture()
    {
        this.HttpContextAccessor = new Mock<IHttpContextAccessor>();
        this.InfrastructureService = new InfrastructureService(this.HttpContextAccessor.Object);
    }

    #region Properties

    public IInfrastructureService InfrastructureService { get; private set; }

    public Mock<IHttpContextAccessor> HttpContextAccessor { get; private set; }

    #endregion
}
