using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AdvancedSystems.Backend.Models;
using AdvancedSystems.Backend.Models.Interfaces;

namespace AdvancedSystems.Backend.Service;

public class BookService : IBookService
{
    private static List<Book> Books { get; set; } = new()
    {
        new Book{ Id = 1, Author = "John R. Taylor", Title = "Classical Mechanics" },
        new Book{ Id = 2, Author = "H. M. Schey", Title = "div, grad, curl and all that" },
        new Book{ Id = 3, Author = "G. Stephenson", Title = "Mathematical Methods for Science Students" },
    };

    static BookService()
    {

    }

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

    public async Task<Book?> GetById(int id)
    {
        return await Task.Run(() => Books.FirstOrDefault(b => b.Id == id));
    }

    public async Task Update(int id, Book book)
    {
        int index = await Task.Run(() => Books.FindIndex(b => b.Id == id));
        if (index == -1) return;
        Books[index] = book;
    }

    public async Task Delete(int id)
    {
        var book = await GetById(id);
        if (book is null) return;
        Books.Remove(book);
    }

    #endregion CRUD
}
