using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using Web_153502_Tolstoi.API.Data;
using Web_153502_Tolstoi.API.Services;
using Web_153502_Tolstoi.Domain.Entities;
using Web_153502_Tolstoi.Domain.Models;

namespace Web_153502_Tolstoi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        IGameService _gameService;
        private ILogger<GamesController> _logger;
        private string _imagesPath;
        private string? _appUri;

        public GamesController(IGameService gameService, IWebHostEnvironment env,
            IConfiguration configuration,
            ILogger<GamesController> logger)
        {
            _gameService = gameService;
            _logger = logger;
            _imagesPath = Path.Combine(env.WebRootPath, "Images");
            _appUri = configuration.GetSection("appUri").Value;
        }

        // GET: api/Games
        [HttpGet()]
        public async Task<ActionResult<ResponseData<ListModel<Game>>>> GetGames()
        {
            return Ok(await _gameService.GetFullGameListAsync());
        }

        // GET: api/Games/category/pageNo
        [HttpGet("{category}/page{pageNo}")]
        public async Task<ActionResult<ResponseData<ListModel<Game>>>> GetGames(string category, int pageNo = 1, int pageSize = 3)
        {
            return Ok(await _gameService.GetGameListAsync(category, pageNo, pageSize));
        }

        // GET: api/Games/category
        [HttpGet("{category}")]
        public async Task<ActionResult<ResponseData<ListModel<Game>>>> GetGames(string category)
        {
            return Ok(await _gameService.GetGameListAsync(category, 1, 3));
        }

        // GET: api/Games/pageNo
        [HttpGet("page{pageNo}")]
        public async Task<ActionResult<ResponseData<ListModel<Game>>>> GetGames(int pageNo = 1, int pageSize = 3)
        {
            return Ok(await _gameService.GetGameListAsync(null, pageNo, pageSize));
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var games = (await _gameService.GetGameListAsync(null)).Data.Items;
            if (games == null)
            {
                return NotFound();
            }
            var game = (games.FirstOrDefault(m => m.Id == id));

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // POST: api/Games/5
        [HttpPost("{id}")]
        public async Task<ActionResult<ResponseData<string>>> PostImage(int id,
            IFormFile formFile)
        {
            var response = await _gameService.SaveImageAsync(id, formFile);
            if (response.Success)
            {
            }
            return NotFound(response);
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

            try
            {
                await _gameService.UpdateGameAsync(id, game);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await GameExists(id)))
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
            await _gameService.CreateGameAsync(game);

            return CreatedAtAction("PostGame", new { id = game.Id }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            await _gameService.DeleteGameAsync(id);

            return NoContent();
        }

        private async Task<bool> GameExists(int id)
        {
            if ((await _gameService.GetGameByIdAsync(id)) != null)
            {
                return true;
            }
            return false;
        }
    }
}
