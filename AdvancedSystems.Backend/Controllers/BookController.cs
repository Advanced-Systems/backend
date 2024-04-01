using System.Collections.Generic;
using System.Threading.Tasks;

using AdvancedSystems.Backend.Configuration.Settings;
using AdvancedSystems.Backend.Interfaces;
using AdvancedSystems.Backend.Models;

using Asp.Versioning;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AdvancedSystems.Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BookController(IBookService bookService, IOptions<AppSettings> appSettings, ILogger<BookController> logger) : ControllerBase
    {
        private readonly IBookService _bookService = bookService;

        private readonly AppSettings _appSettings = appSettings.Value;

        private readonly ILogger<BookController> _logger = logger;

        #region CRUD

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            await _bookService.Add(book);
            this._logger.LogDebug("Created book: {}", book);
            return CreatedAtAction(nameof(Get), new { Id = book.Id }, book);
        }

        [HttpGet]
        [ApiVersion("1")]
        [ApiVersion("2")]
        public async Task<ActionResult<List<Book>>> GetAll()
        {
            this._logger.LogDebug("Retrieve all books");
            return Ok(await _bookService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Book>> Get(int id)
        {
            var book = await _bookService.GetByIdAsync(id);

            if (book is null)
            {
                this._logger.LogDebug("Failed to retrieve book: {}", book);
                return NotFound();
            }

            this._logger.LogDebug("Retrieved book: {}", book);
            return Ok(book);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, Book book)
        {
            if (id != book.Id)
                return BadRequest();

            var oldBook = await _bookService.GetByIdAsync(id);

            if (oldBook is null)
            {
                this._logger.LogDebug("Failed to update book: {}", book);
                return NotFound();
            }

            await _bookService.Update(id, book);
            this._logger.LogDebug("Updated book: {}", book);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookService.GetByIdAsync(id);

            if (book is null)
            {
                this._logger.LogDebug("Failed to remove book: {}", book);
                return NotFound();
            }

            await _bookService.Delete(id);
            this._logger.LogDebug("Removed book: {}", book);

            return NoContent();
        }

        #endregion CRUD
    }
}
