using System.Threading.Tasks;

using AdvancedSystems.Backend.Service;
using AdvancedSystems.Backend.Models.Interfaces;

using Moq;
using Xunit;

namespace AdvancedSystems.Backend.Tests;

public class BookServiceTests
{
    public BookServiceTests()
    {
        _serviceUnderTest = new BookService(_mockLogger.Object);
    }

    private readonly BookService _serviceUnderTest;

    private readonly Mock<ILoggingService> _mockLogger = new Mock<ILoggingService>();

    #region Unit Tests

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBook()
    {
        // Arrange
        int id = 1;

        // Act
        var book = await _serviceUnderTest.GetByIdAsync(id);

        // Assert
        Assert.Equal(id, book?.Id);
    }

    #endregion
}
