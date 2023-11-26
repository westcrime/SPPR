using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
        [AllowAnonymous]
        public async Task<ActionResult<ResponseData<ListModel<Game>>>> GetGames()   
        {
            var response = await _gameService.GetFullGameListAsync();
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // GET: api/Games/category/pageNo
        [HttpGet("{category}/page{pageNo}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseData<ListModel<Game>>>> GetGames(string category, int pageNo = 1, int pageSize = 3)
        {
            var response = await _gameService.GetGameListAsync(category, pageNo, pageSize);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // GET: api/Games/category
        [HttpGet("{category}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseData<ListModel<Game>>>> GetGames(string category)
        {
            var response = await _gameService.GetGameListAsync(category, 1, 3);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // GET: api/Games/pageNo
        [HttpGet("page{pageNo}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseData<ListModel<Game>>>> GetGames(int pageNo = 1, int pageSize = 3)
        {
            var response = await _gameService.GetGameListAsync(null, pageNo, pageSize);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseData<Game>>> GetGame(int id)
        {
            var response = await _gameService.GetGameByIdAsync(id);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // POST: api/Games/5
        [HttpPost("{id}")]
        public async Task<ActionResult<ResponseData<string>>> PostImage(int id,
            IFormFile formFile)
        {
            var response = await _gameService.SaveImageAsync(id, formFile);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseData<Game>>> PutGame(int id, Game game)
        {
            var response = await _gameService.UpdateGameAsync(id, game);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            var response = await _gameService.CreateGameAsync(game);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseData<bool>>> DeleteGame(int id)
        {
            var response = await _gameService.DeleteGameAsync(id);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
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
