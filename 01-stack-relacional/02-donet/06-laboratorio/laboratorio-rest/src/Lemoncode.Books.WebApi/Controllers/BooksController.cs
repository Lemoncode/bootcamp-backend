using Lemoncode.Books.Application.Models;
using Lemoncode.Books.Application.Models.Filters;
using Lemoncode.Books.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lemoncode.Books.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class BooksController
        : ControllerBase
    {
        private readonly CommandService _commandService;
        private readonly QueryService _queryService;

        public BooksController(
            CommandService commandService,
            QueryService queryService)
        {
            _commandService = commandService;
            _queryService = queryService;
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] NewBook newBook)
        {
            _commandService.CreateBook(newBook);
            return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateBook(int id, [FromBody] UpdateBook updateBook)
        {
            if (id != updateBook.Id)
            {
                return BadRequest("Id does not match");
            }
            _commandService.UpdateBook(updateBook);
            return Ok(updateBook);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetBook(int id)
        {
            var book = _queryService.GetBook(id);
            return Ok(book);
        }

        [HttpGet]
        public IActionResult GetBooks([FromQuery] BooksFilter booksFilter)
        {
            var books = _queryService.GetBooks(booksFilter);
            return Ok(books);
        }
    }
}
