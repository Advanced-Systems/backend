using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AdvancedSystems.Backend.Interfaces;
using AdvancedSystems.Backend.Models;

using Asp.Versioning;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Mime;

namespace AdvancedSystems.Backend.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Produces(MediaTypeNames.Application.Json)]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly ILogger<BookController> _logger;
    public BookController(IBookService bookService, ILogger<BookController> logger)
    {
        _bookService = bookService;
        _logger = logger;
    }

    #region CRUD

    [HttpPost]
    public async Task<IActionResult> Create(Book book)
    {
        await _bookService.Add(book);
        _logger.LogDebug("Created book: {}", book);
        return CreatedAtAction(nameof(Get), new { book.Id }, book);
    }

    [HttpGet]
    [ApiVersion(1.0)]
    [ApiVersion(2.0)]
    public async Task<ActionResult<List<Book>>> GetAll()
    {
        _logger.LogDebug("Retrieve all books");
        return Ok(await _bookService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Book>> Get(int id)
    {
        var book = await _bookService.GetByIdAsync(id);

        if (book is null)
        {
            _logger.LogDebug("Failed to retrieve book: {}", book);
            return NotFound();
        }

        if (book.Id == 2)
        {
            throw new ArgumentException("Provoke exception handler");
        }

        _logger.LogDebug("Retrieved book: {}", book);
        return Ok(book);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, Book book)
    {
        if (id != book.Id)
            return BadRequest();

        var oldBook = await _bookService.GetByIdAsync(id);

        if (oldBook is null)
        {
            _logger.LogDebug("Failed to update book: {}", book);
            return NotFound();
        }

        await _bookService.Update(id, book);
        _logger.LogDebug("Updated book: {}", book);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _bookService.GetByIdAsync(id);

        if (book is null)
        {
            _logger.LogDebug("Failed to remove book: {}", book);
            return NotFound();
        }

        await _bookService.Delete(id);
        _logger.LogDebug("Removed book: {}", book);

        return NoContent();
    }

    #endregion CRUD
}