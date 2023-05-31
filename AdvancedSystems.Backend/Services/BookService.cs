using System.Collections.Generic;
using System.Linq;

using AdvancedSystems.Backend.Models;

namespace AdvancedSystems.Backend.Service;

public static class BookService
{
    private static List<Book> Books { get; set; }
    private static int nextId;

    static BookService()
    {
        Books = new List<Book>()
        {
            new Book{ Id = 1, Author = "John R. Taylor", Title = "Classical Mechanics" },
            new Book{ Id = 2, Author = "H. M. Schey", Title = "div, grad, curl and all that" },
            new Book{ Id = 3, Author = "G. Stephenson", Title = "Mathematical Methods for Science Students" },
        };

        nextId = Books.Count;
    }

    #region CRUD

    public static void Add(Book book)
    {
        book.Id = nextId++;
        Books.Add(book);
    }

    public static List<Book> GetAll() => Books;

    public static Book? Get(int id) => Books.FirstOrDefault(b => b.Id == id);

    public static void Update(int id, Book book)
    {
        int index = Books.FindIndex(b => b.Id == id);
        if (index == -1) return;
        Books[index] = book;
    }

    public static void Delete(int id)
    {
        var book = Get(id);
        if (book is null) return;
        Books.Remove(book);
    }

    #endregion CRUD
}
