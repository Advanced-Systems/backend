using System.Threading.Tasks;

using AdvancedSystems.Backend.Services;
using AdvancedSystems.Backend.Interfaces;

using Moq;
using Xunit;

namespace AdvancedSystems.Backend.Tests;

public class BookServiceTests
{
    private readonly BookService _serviceUnderTest;

    public BookServiceTests()
    {
        _serviceUnderTest = new BookService();
    }

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
