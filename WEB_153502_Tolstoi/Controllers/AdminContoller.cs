using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_153502_Tolstoi.API.Data;
using Web_153502_Tolstoi.API.Services;
using Web_153502_Tolstoi.Domain.Entities;

namespace WEB_153502_Tolstoi.Views.Admin
{
    public class AdminController : Controller
    {
        private readonly IGameService _context;

        public AdminController(IGameService context)
        {
            _context = context;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            return _context.GetGameListAsync(null) != null ?
                        View((await _context.GetGameListAsync(null)).Data.Items.ToList()) :
                        Problem("Entity set 'AppDbContext.Games'  is null.");
        }

        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GetGameListAsync(null) == null)
            {
                return NotFound();
            }

            var game = (await _context.GetGameListAsync(null)).Data.Items
                .FirstOrDefault(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CategoryId,Price,Image")] Game game)
        {
            if (ModelState.IsValid)
            {
                await _context.CreateGameAsync(game);
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        // GET: AdminGames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || (await _context.GetGameListAsync(null)).Data.Items == null)
            {
                return NotFound();
            }

            var game = (await _context.GetGameListAsync(null)).Data.Items.FirstOrDefault(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        // POST: AdminGames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CategoryId,Price,Image")] Game game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdateGameAsync(id, game);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await GameExists(game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        // GET: AdminGames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || (await _context.GetGameListAsync(null)).Data.Items == null)
            {
                return NotFound();
            }

            var game = (await _context.GetGameListAsync(null)).Data.Items
                .FirstOrDefault(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if ((await _context.GetGameListAsync(null)).Data.Items == null)
            {
                return Problem("Entity set 'AppDbContext.Games'  is null.");
            }
            var game = (await _context.GetGameListAsync(null)).Data.Items.FirstOrDefault(m => m.Id == id);
            if (game != null)
            {
                (await _context.GetGameListAsync(null)).Data.Items.Remove(game);
            }

            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> GameExists(int id)
        {
            return (((await _context.GetGameListAsync(null)).Data.Items?.Any(e => e.Id == id)).GetValueOrDefault());
        }
    }
}
