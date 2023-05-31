using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AdvancedSystems.Backend.Models;
using AdvancedSystems.Backend.Models.Interfaces;

namespace AdvancedSystems.Backend.Service;

public class BookService : IBookService
{
    public BookService(ILoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    private readonly ILoggingService _loggingService;

    private static List<Book> Books { get; set; } = new()
    {
        new Book{ Id = 1, Author = "John R. Taylor", Title = "Classical Mechanics" },
        new Book{ Id = 2, Author = "H. M. Schey", Title = "div, grad, curl and all that" },
        new Book{ Id = 3, Author = "G. Stephenson", Title = "Mathematical Methods for Science Students" },
    };

    #region CRUD

    public async Task Add(Book book)
    {
        book.Id = Books.Count + 1;
        _loggingService.LogDebug("Create book: {}", book.Title!);
        await Task.Run(() => Books.Add(book));
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        _loggingService.LogDebug("Get all books");
        return await Task.Run(() => Books);
    }

    public async Task<Book?> GetById(int id)
    {
        var book = await Task.Run(() => Books.FirstOrDefault(b => b.Id == id));
        _loggingService.LogDebug("Get book: {}", book?.Title ?? "n/a");
        return book;
    }

    public async Task Update(int id, Book book)
    {
        int index = await Task.Run(() => Books.FindIndex(b => b.Id == id));
        if (index == -1) return;
        _loggingService.LogDebug("Updated book: {}", book.Title!);
        Books[index] = book;
    }

    public async Task Delete(int id)
    {
        var book = await GetById(id);
        if (book is null) return;
        _loggingService.LogDebug("Deleted book: {}", book.Title!);
        Books.Remove(book);
    }

    #endregion CRUD
}
