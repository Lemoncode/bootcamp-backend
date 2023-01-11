using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lemoncode.Azure.Api.Data;
using Lemoncode.Azure.Api.Models;

namespace Lemoncode.Azure.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly ApiDBContext context;

        public GamesController(ApiDBContext context)
        {
            this.context = context;
        }



        // GET: api/Games
        [HttpGet("healthcheck")]
        public async Task<ActionResult<IEnumerable<Game>>> HealthCheck()
        {
            return Ok("Service is running and healthy");
        }


        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGame()
        {
            return await context.Game.ToListAsync();
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await context.Game.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            context.Entry(game).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            context.Game.Add(game);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetGame", new { id = game.Id }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            context.Game.Remove(game);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameExists(int id)
        {
            return context.Game.Any(e => e.Id == id);
        }
    }
}
