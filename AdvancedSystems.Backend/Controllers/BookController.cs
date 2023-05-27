using AdvancedSystems.Backend.Models;
using AdvancedSystems.Backend.Configuration.Settings;
using AdvancedSystems.Backend.Service;

using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AdvancedSystems.Backend.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BookController : BaseController
{
    public BookController(ILogger<BookController> logger, IOptions<AppSettings> configuration) : base(logger)
    {
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));

        AppSettings = configuration.Value;
    }

    private AppSettings AppSettings { get; }

    #region CRUD

    [HttpPost]
    public IActionResult Create(Book book)
    {
        BookService.Add(book);
        Logger.LogDebug($"Create book '{book.Title}'");
        return CreatedAtAction(nameof(Get), new { Id = book.Id }, book);
    }

    [HttpGet]
    public ActionResult<List<Book>> GetAll()
    {
        Logger.LogDebug($"Test Configuration: {AppSettings.Test}");
        Logger.LogDebug("Get all books");
        return BookService.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Book> Get(int id)
    {
        var book = BookService.Get(id);
        if (book is null) return NotFound();
        Logger.LogDebug($"Get book '{book.Title}'");
        return book;
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Book book)
    {
        if (id != book.Id) return BadRequest();

        var oldBook = BookService.Get(id);
        if (oldBook is null) return NotFound();

        Logger.LogDebug($"Update book '{book.Title}'");
        BookService.Update(id, book);
        return NoContent();
    }

    [HttpDelete("{id}")]
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
