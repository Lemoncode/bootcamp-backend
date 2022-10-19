using System;
using Lemoncode.Soccer.Application.Models;
using Lemoncode.Soccer.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lemoncode.Soccer.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController 
        : ControllerBase
    {
        private readonly GamesCommandService _gamesCommandService;
        private readonly GamesQueryService _gamesQueryService;
        
        public GamesController(
            GamesCommandService gamesCommandService,
            GamesQueryService gamesQueryService)
        {
            _gamesCommandService = gamesCommandService;
            _gamesQueryService = gamesQueryService;
        }
        
        [HttpGet]
        public IActionResult GetGameReports()
        {
            var gameReports = _gamesQueryService.GetGameReports();
            return Ok(gameReports);
        }

        /// <param name="id" example="00000000-0000-0000-0000-000000000000">The game id</param>
        /// <param name="isDetailed" example="true">Flag that indicates whether the report is detailed or not</param>
        [HttpGet("{id}")]
        public IActionResult GetGameReport(Guid id, [FromQuery] bool isDetailed = false)
        {
            var gameReport = _gamesQueryService.GetGameReport(id, isDetailed);
            return Ok(gameReport);
        }
        
        /// <param name="id" example="00000000-0000-0000-0000-000000000000">The game id</param>
        [HttpDelete("{id}")]
        public IActionResult DeleteGame(Guid id)
        {
            _gamesQueryService.DeleteGame(id);
            return Ok();
        }

        [HttpPost]
        public IActionResult AddGame([FromBody] NewGame newGame)
        {
            var id = _gamesCommandService.CreateGame(newGame);
            return CreatedAtAction(nameof(GetGameReport), new { id = id}, newGame);
        }

        /// <param name="id" example="00000000-0000-0000-0000-000000000000">The game id</param>
        /// <param name="gameProgress">The patch game object containing the isInProgress property</param>
        [HttpPatch("{id}")]
        public IActionResult StartGame(Guid id, [FromBody] GameProgress gameProgress)
        {
            _gamesCommandService.SetProgress(id, gameProgress);
            return Ok();
        }

        /// <param name="id" example="00000000-0000-0000-0000-000000000000">The game id</param>
        /// <param name="newGoal">The new goal containing the team that scores and the scorer</param>
        [HttpPost("{id}/goals")]
        public IActionResult AddGoal(Guid id, [FromBody] NewGoal newGoal)
        {
            _gamesCommandService.AddGoal(id, newGoal);
            return Ok();
        }
    }
}