using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AdvancedSystems.Backend.Configuration.Settings;
using AdvancedSystems.Backend.Models;
using AdvancedSystems.Backend.Models.Interfaces;

using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AdvancedSystems.Backend.Controllers;

[ApiController]
[ApiVersion("1")]
[ApiExplorerSettings(GroupName = "Library")]
[Produces("application/json")]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    public BookController(IBookService bookService, IOptions<AppSettings> appSettings, ILogger<BookController> logger)
    {
        ArgumentNullException.ThrowIfNull(appSettings);

        _bookService = bookService;
        _appSettings = appSettings.Value;
        _logger = logger;

        _logger.LogInformation("Announce BookService!");
    }

    private readonly IBookService _bookService;

    private readonly ILogger<BookController> _logger;

    private readonly AppSettings _appSettings;

    #region CRUD

    [HttpPost]
    public async Task<IActionResult> Create(Book book)
    {
        await _bookService.Add(book);
        return CreatedAtAction(nameof(Get), new { Id = book.Id }, book);
    }

    [HttpGet]
    [ApiVersion("1")]
    [ApiVersion("2")]
    public async Task<ActionResult<List<Book>>> GetAll()
    {
        return Ok(await _bookService.GetAllAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Book>> Get(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book is null) return NotFound();
        return Ok(book);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, Book book)
    {
        if (id != book.Id) return BadRequest();

        var oldBook = await  _bookService.GetByIdAsync(id);
        if (oldBook is null) return NotFound();

        await _bookService.Update(id, book);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _bookService.GetByIdAsync(id);

        if (book is null) return NotFound();

        await _bookService.Delete(id);
        return NoContent();
    }

    #endregion CRUD
}
