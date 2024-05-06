using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AdvancedSystems.Backend.Interfaces;
using AdvancedSystems.Backend.Models;

using Microsoft.Extensions.Logging;

namespace AdvancedSystems.Backend.Services;

public class BookService(ILogger<IBookService> logger) : IBookService
{
    private readonly ILogger<IBookService> _logger = logger;

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
        await Task.Run(() => Books.Add(book));
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await Task.Run(() => Books);
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        var book = await Task.Run(() => Books.FirstOrDefault(b => b.Id == id));
        return book;
    }

    public async Task Update(int id, Book book)
    {
        int index = await Task.Run(() => Books.FindIndex(b => b.Id == id));
        if (index == -1) return;
        Books[index] = book;
    }

    public async Task Delete(int id)
    {
        var book = await GetByIdAsync(id);
        if (book is null) return;
        Books.Remove(book);
    }

    #endregion CRUD
}