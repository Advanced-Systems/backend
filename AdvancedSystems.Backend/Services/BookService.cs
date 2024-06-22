using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AdvancedSystems.Backend.Interfaces;
using AdvancedSystems.Backend.Models.API;

using Microsoft.Extensions.Logging;

namespace AdvancedSystems.Backend.Services;

public sealed class BookService : IBookService
{
    private readonly ILogger<IBookService> _logger;
    
    public BookService(ILogger<IBookService> logger)
    {
        _logger = logger;
    }

    private static List<Book> Books { get; } =
    [
        new Book { Id = 1, Author = "John R. Taylor", Title = "Classical Mechanics" },
        new Book { Id = 2, Author = "H. M. Schey", Title = "div, grad, curl and all that" },
        new Book { Id = 3, Author = "G. Stephenson", Title = "Mathematical Methods for Science Students" }
    ];

    #region CRUD

    public async Task Add(Book book)
    {
        book.Id = Books.Count + 1;
        this._logger.LogDebug("Add book: {Book}.", book);
        await Task.FromResult(() => Books.Add(book));
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        this._logger.LogDebug("Get all books: {Books}.", Books);
        return await Task.FromResult(Books);
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        var book = await Task.FromResult(Books.FirstOrDefault(b => b.Id == id));
        this._logger.LogDebug("Get Book by Id: {Id} (book={Book}).", id, book);
        return book;
    }

    public async Task Update(int id, Book book)
    {
        int index = await Task.FromResult(Books.FindIndex(b => b.Id == id));
        
        if (index == -1) return;
        
        this._logger.LogDebug("Update book: {Book}.", book);
        Books[index] = book;
    }

    public async Task Delete(int id)
    {
        var book = await GetByIdAsync(id);
        if (book is null) return;
        
        this._logger.LogDebug("Remove book: {Book}.", book);
        Books.Remove(book);
    }

    #endregion
}