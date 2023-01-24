using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Lemoncode.Azure.Api.Data;
using Lemoncode.Azure.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using NuGet.Protocol;

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
            return await context.Game.Include(i => i.Screenshots).ToListAsync();
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

        [HttpPost("{id}/Screenshots/Upload")]
        public async Task<IActionResult> UploadFile([FromRoute] int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            try
            {
                var storageUrl = "https://lemoncodeazurestg.blob.core.windows.net/";
                var blobName = $"sf2/{file.FileName}";
                var containerName = "screenshots";
                var connectionString = "DefaultEndpointsProtocol=https;AccountName=lemoncodeazurestg;AccountKey=h6jIAW8j0p4oBvs9Eh71+0x4bqsfSP+WBYWRIMuoiBLjnAJJYQ5cmzffHy9FpgW/HN2mGB1BzeiQ+ASt4QQ88Q==;EndpointSuffix=core.windows.net";
                BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
                await container.CreateIfNotExistsAsync();
                BlobClient blobClient = container.GetBlobClient(blobName);
                BlobContentInfo blobInfo = null;
                bool fileExists = false;

                if (await blobClient.ExistsAsync())
                {
                    fileExists = true;
                }

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    if (!fileExists)
                    {
                        blobInfo = await blobClient.UploadAsync(memoryStream);
                    }
                    //return Conflict("El fichero ya existe");
                }

                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                    BlobName = blobClient.Name,
                    Resource = "b",
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
                };
                sasBuilder.SetPermissions(BlobSasPermissions.Read |
                        BlobSasPermissions.Write);
                var blobSasUri = blobClient.GenerateSasUri(sasBuilder);
                var game = context.Game.Include(i => i.Screenshots)
                    .FirstOrDefault(i => i.Id == id);
                context.Game.Add(game);

                if (!fileExists)
                {
                    if (game.Screenshots == null)
                    {
                        game.Screenshots = new List<Screenshot>();
                    }
                    game.Screenshots.Add(
                        new Screenshot { 
                            Url = $"{storageUrl}screenshots/{blobName}" ,
                            Filename = file.FileName
                        });
                }
                else
                {
                    var screenshot = game.Screenshots.FirstOrDefault(i => i.Filename == file.FileName);
                    screenshot.Url = blobSasUri.AbsoluteUri;
                }

                context.Game.Update(game);
                context.SaveChanges();
                return Ok(blobSasUri);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
