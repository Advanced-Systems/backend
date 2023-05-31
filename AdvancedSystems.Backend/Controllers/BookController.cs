using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AdvancedSystems.Backend.Base;
using AdvancedSystems.Backend.Configuration.Settings;
using AdvancedSystems.Backend.Models;
using AdvancedSystems.Backend.Models.Interfaces;

using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace AdvancedSystems.Backend.Controllers;

[ApiController]
[ApiVersion("1")]
[ApiExplorerSettings(GroupName = "Library")]
[Produces("application/json")]
[Route("api/[controller]")]
public class BookController : BaseController
{
    private readonly IBookService _bookService;

    public BookController(ILogger<BookController> logger, IOptions<AppSettings> configuration, IBookService bookService) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        _bookService = bookService;
        AppSettings = configuration.Value;
    }

    private AppSettings AppSettings { get; }

    #region CRUD

    [HttpPost]
    public async Task<IActionResult> Create(Book book)
    {
        await _bookService.Add(book);
        Logger.LogDebug($"Create book '{book.Title}'");
        return CreatedAtAction(nameof(Get), new { Id = book.Id }, book);
    }

    [HttpGet]
    [ApiVersion("2")]
    public async Task<ActionResult<List<Book>>> GetAll()
    {
        Logger.LogDebug("Get all books");
        return Ok(await _bookService.GetAllAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Book>> Get(int id)
    {
        var book = await _bookService.GetById(id);
        if (book is null) return NotFound();
        Logger.LogDebug($"Get book '{book.Title}'");
        return Ok(book);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, Book book)
    {
        if (id != book.Id) return BadRequest();

        var oldBook = await  _bookService.GetById(id);
        if (oldBook is null) return NotFound();

        Logger.LogDebug($"Update book '{book.Title}'");
        await _bookService.Update(id, book);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _bookService.GetById(id);

        if (book is null) return NotFound();

        Logger.LogDebug($"Delete book '{book.Title}'");
        await _bookService.Delete(id);
        return NoContent();
    }

    #endregion CRUD
}
