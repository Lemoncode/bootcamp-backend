using Lemoncode.Books.Application.Models;
using Lemoncode.Books.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lemoncode.Books.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthorsController
        : ControllerBase
    {
        private readonly CommandService _commandService;
        private readonly QueryService _queryService;

        public AuthorsController(
            CommandService commandService,
            QueryService queryService)
        {
            _commandService = commandService;
            _queryService = queryService;
        }

        [HttpPost]
        public IActionResult CreateAuthor([FromBody] NewAuthor newAuthor)
        {
            _commandService.CreateAuthor(newAuthor);
            return CreatedAtAction(nameof(GetAuthor), new {id = newAuthor.Id}, newAuthor);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetAuthor(int id)
        {
            var author = _queryService.GetAuthor(id);
            return Ok(author);
        }
    }
}
