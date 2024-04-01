using System.Threading.Tasks;

using AdvancedSystems.Backend.Interfaces;

using Moq;
using Xunit;
using AdvancedSystems.Backend.Models;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedSystems.Backend.Tests;

public class BookServiceTests
{
    private readonly Mock<IBookService> _bookServiceMock;

    private List<Book> _books { get; } =
        [
            new Book { Id = 1, Author = "John R. Taylor", Title = "Classical Mechanics" },
            new Book { Id = 2, Author = "H. M. Schey", Title = "div, grad, curl and all that" },
            new Book { Id = 3, Author = "G. Stephenson", Title = "Mathematical Methods for Science Students" }
        ];

    public BookServiceTests()
    {
        this._bookServiceMock = new Mock<IBookService>();
        this._bookServiceMock.Setup(m => m.GetAllAsync()).Returns(Task.FromResult(this._books.AsEnumerable()));
    }

    #region Unit Tests

    [Fact]
    public async Task GetAllAsync_ShouldReturnBook()
    {
        // Arrange
        int id = 1;

        // Act
        var books = await _bookServiceMock.Object.GetAllAsync();

        // Assert
        Assert.Equal(id, books.Count(x => x.Id == id));
    }

    #endregion
}
