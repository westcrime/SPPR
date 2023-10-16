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

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        // GET: api/Games
        [HttpGet()]
        public async Task<ActionResult<ResponseData<ListModel<Game>>>> GetGames()
        {
            return Ok(await _gameService.GetGameListAsync(null, 1, 3));
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

        //// GET: api/Games/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Game>> GetGame(int id)
        //{
        //    //if (_context.Games == null)
        //    //{
        //    //    return NotFound();
        //    //}
        //    //  var game = await _context.Games.FindAsync(id);

        //    //  if (game == null)
        //    //  {
        //    //      return NotFound();
        //    //  }

        //    //  return game;
        //    throw new NotImplementedException();
        //}

        //// PUT: api/Games/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutGame(int id, Game game)
        //{
        //    //if (id != game.Id)
        //    //{
        //    //    return BadRequest();
        //    //}

        //    //_context.Entry(game).State = EntityState.Modified;

        //    //try
        //    //{
        //    //    await _context.SaveChangesAsync();
        //    //}
        //    //catch (DbUpdateConcurrencyException)
        //    //{
        //    //    if (!GameExists(id))
        //    //    {
        //    //        return NotFound();
        //    //    }
        //    //    else
        //    //    {
        //    //        throw;
        //    //    }
        //    //}

        //    //return NoContent();
        //    throw new NotImplementedException();
        //}

        //// POST: api/Games
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Game>> PostGame(Game game)
        //{
        //    //if (_context.Games == null)
        //    //{
        //    //    return Problem("Entity set 'AppDbContext.Games'  is null.");
        //    //}
        //    //  _context.Games.Add(game);
        //    //  await _context.SaveChangesAsync();

        //    //  return CreatedAtAction("GetGame", new { id = game.Id }, game);
        //    throw new NotImplementedException();
        //}

        //// DELETE: api/Games/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteGame(int id)
        //{
        //    //if (_context.Games == null)
        //    //{
        //    //    return NotFound();
        //    //}
        //    //var game = await _context.Games.FindAsync(id);
        //    //if (game == null)
        //    //{
        //    //    return NotFound();
        //    //}

        //    //_context.Games.Remove(game);
        //    //await _context.SaveChangesAsync();

        //    //return NoContent();
        //    throw new NotImplementedException();
        //}

        //private bool GameExists(int id)
        //{
        //    //return (_context.Games?.Any(e => e.Id == id)).GetValueOrDefault();
        //    throw new NotImplementedException();
        //}
    }
}
