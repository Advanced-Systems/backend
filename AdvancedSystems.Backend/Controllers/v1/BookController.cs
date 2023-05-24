using AdvancedSystems.Backend.Contracts;
using AdvancedSystems.Backend.Models;
using AdvancedSystems.Backend.Models.Core;
using AdvancedSystems.Backend.Service;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AdvancedSystems.Backend.Controllers;

[ApiController]
public class BookController : BaseController
{
    public BookController(ILogger<BookController> logger, IOptions<AppSettings> configuration) : base(logger)
    {
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));

        AppSettings = configuration.Value;
    }

    private AppSettings AppSettings { get; }

    #region CRUD

    [HttpPost(ApiRoutes.BookVersion1.Create)]
    public IActionResult Create(Book book)
    {
        BookService.Add(book);
        Logger.LogDebug($"Create book '{book.Title}'");
        return CreatedAtAction(nameof(Get), new { Id = book.Id }, book);
    }

    [HttpGet(ApiRoutes.BookVersion1.GetAll)]
    public ActionResult<List<Book>> GetAll()
    {
        Logger.LogDebug($"Test Configuration: {AppSettings.Test}");
        Logger.LogDebug("Get all books");
        return BookService.GetAll();
    }

    [HttpGet(ApiRoutes.BookVersion1.Get)]
    public ActionResult<Book> Get(int id)
    {
        var book = BookService.Get(id);
        if (book is null) return NotFound();
        Logger.LogDebug($"Get book '{book.Title}'");
        return book;
    }

    [HttpPut(ApiRoutes.BookVersion1.Update)]
    public IActionResult Update(int id, Book book)
    {
        if (id != book.Id) return BadRequest();

        var oldBook = BookService.Get(id);
        if (oldBook is null) return NotFound();

        Logger.LogDebug($"Update book '{book.Title}'");
        BookService.Update(id, book);
        return NoContent();
    }

    [HttpDelete(ApiRoutes.BookVersion1.Delete)]
    public IActionResult Delete(int id)
    {
        var book = BookService.Get(id);

        if (book is null) return NotFound();

        Logger.LogDebug($"Delete book '{book.Title}'");
        BookService.Delete(id);
        return NoContent();
    }

    #endregion CRUD
}
