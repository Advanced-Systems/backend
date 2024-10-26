using AdvancedSystems.Backend.Tests.Fixtures;

using Asp.Versioning;

using Microsoft.AspNetCore.Http;

using Xunit;

namespace AdvancedSystems.Backend.Tests.Services;

public sealed class InfrastructureServiceTests : IClassFixture<InfrastructureServiceFixture>
{
    private readonly InfrastructureServiceFixture _sut;

    public InfrastructureServiceTests(InfrastructureServiceFixture infrastructureServiceFixture)
    {
        this._sut = infrastructureServiceFixture;
    }

    #region Tests

    /// <summary>
    ///     Tests that the requested API versions returns a non-nullable value.
    /// </summary>
    [Fact]
    public void TestRequestedApiVersion()
    {
        // Arrange
        var expectedVersion = new ApiVersion(2.1);
        var httpContext = new DefaultHttpContext();
        var featureCollection = new ApiVersioningFeature(httpContext);
        featureCollection.RequestedApiVersion = expectedVersion;
        httpContext.Features.Set<IApiVersioningFeature>(featureCollection);

        this._sut.HttpContextAccessor
            .Setup(x => x.HttpContext)
            .Returns(httpContext);

        // Act
        var apiVersion = this._sut.InfrastructureService.RequestedApiVersion;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(apiVersion);
            Assert.Equal(expectedVersion, apiVersion);
        });
    }

    #endregion
}