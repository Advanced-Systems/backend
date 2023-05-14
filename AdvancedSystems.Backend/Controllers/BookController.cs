using AdvancedSystems.Backend.Models;
using AdvancedSystems.Backend.Service;

using Microsoft.AspNetCore.Mvc;

namespace AdvancedSystems.Backend.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BookController : ControllerBase
{
    public BookController()
    {

    }

    #region CRUD

    [HttpPost]
    public IActionResult Create(Book book)
    {
        BookService.Add(book);
        return CreatedAtAction(nameof(Get), new { Id = book.Id }, book);
    }

    [HttpGet]
    public ActionResult<List<Book>> GetAll() => BookService.GetAll();

    [HttpGet("{id}")]
    public ActionResult<Book> Get(int id)
    {
        var book = BookService.Get(id);
        if (book is null) return NotFound();
        return book;
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Book book)
    {
        if (id != book.Id) return BadRequest();

        var oldBook = BookService.Get(id);
        if (oldBook is null) return NotFound();

        BookService.Update(id, book);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var book = BookService.Get(id);

        if (book is null) return NotFound();

        BookService.Delete(id);
        return NoContent();
    }

    #endregion CRUD
}
