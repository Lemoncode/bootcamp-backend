using Azure.Storage.Queues;
using Lemoncode.Azure.Api.Data;
using Lemoncode.Azure.Api.Helpers;
using Lemoncode.Azure.Api.Services;
using Lemoncode.Azure.Models;
using Lemoncode.Azure.Models.Configuration;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Lemoncode.Azure.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly ApiDBContext context;
        private readonly StorageOptions storageOptions;
        private readonly ILogger log;
        private readonly TelemetryClient telemetry;

        public GamesController(ApiDBContext context,
                                IOptions<StorageOptions> storageOptionsSettings,
                                ILogger<GamesController> log,
                                TelemetryClient telemetry)
        {
            this.context = context;
            this.storageOptions = storageOptionsSettings.Value;
            this.log = log;
            this.telemetry = telemetry;
        }

        // GET: api/Games
        [HttpGet("healthcheck")]
        public async Task<ActionResult<IEnumerable<Game>>> HealthCheck()
        {
            log.LogTrace("GAME - Testing traces");
            log.LogWarning("GAME - Testing Warning");
            log.LogError("GAME - Testing Errors");
            log.LogDebug("GAME - Debug");
            log.LogInformation("GAME - Information");
            log.LogCritical("GAME - Critical");

            telemetry.TrackEvent("GAME - MyCustomEvent");

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
            var game = await context.Game.Include(i => i.Screenshots).FirstOrDefaultAsync(i => i.Id == id);

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

            //context.Game.Remove(game);
            //await context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameExists(int id)
        {
            return context.Game.Any(e => e.Id == id);
        }


        [HttpPost("{id}/Screenshots/Upload")]
        public async Task<IActionResult> UploadScreenshot([FromRoute] int id, IFormFile formFile)
        {
            log.LogInformation($"GAMES - Uploading Screenshot for game with id {id}");
            if (formFile == null || formFile.Length == 0)
            {
                log.LogError($"GAMES - No files received from the upload");
                return BadRequest("No files received from the upload");
            }
            if (storageOptions.AccountKey == string.Empty || storageOptions.AccountName == string.Empty)
            {
                log.LogError($"GAMES - sorry, can't retrieve your azure storage details from appsettings.js, make sure that you add azure storage details there");
                return BadRequest("sorry, can't retrieve your azure storage details from appsettings.js, make sure that you add azure storage details there");
            }
            if (storageOptions.ScreenshotsContainer == string.Empty)
            {
                log.LogError($"GAMES - Please provide a name for your image container in the azure blob storage");
                return BadRequest("Please provide a name for your image container in the azure blob storage");
            }
            await Task.Delay(100);

            try
            {
                if (StorageHelper.IsImage(formFile))
                {
                    if (formFile.Length > 0)
                    {
                        using (Stream stream = formFile.OpenReadStream())
                        {
                            var blobName = $"{id}/{formFile.FileName}";
                            var blobUri = await StorageHelper.UploadFileToStorage(stream, blobName, storageOptions);
                            log.LogInformation($"GAMES - Screenshot uploaded successfully");

                            var game = context.Game.Include(i => i.Screenshots).FirstOrDefault(i => i.Id == id);
                            var newScreenshot = new Screenshot
                            {
                                Filename = formFile.FileName,
                                Url = blobUri,
                                ThumbnailUrl = blobUri.Replace("screenshots", "thumbnails")
                            };
                            context.Game.Add(game);
                            game.Screenshots.Add(newScreenshot);
                            context.Game.Update(game);
                            context.SaveChanges();
                            log.LogInformation($"GAMES - Game updated with Screenshot Url successfully");

                            return Ok(blobUri);
                        }
                    }
                    log.LogError($"GAMES - Empty file");
                    return BadRequest("Empty file");
                }
                else
                {
                    log.LogError($"GAMES - Unsupported Media Type");
                    return new UnsupportedMediaTypeResult();
                }
            }
            catch (Exception ex)
            {
                log.LogError($"GAMES - {ex.Message}");
                return BadRequest(ex.Message);
            }

        }

        private async Task CreateScreenshotMessage(ScreenshotMessage message)
        {
            QueueClient queue = new QueueClient(storageOptions.ConnectionString, storageOptions.ScreenshotsQueue);
            await queue.SendMessageAsync(JsonSerializer.Serialize(message));
        }
    }
}
