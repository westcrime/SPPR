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
using WEB_153502_Tolstoi.Areas.Admin.Models;

namespace WEB_153502_Tolstoi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly IGameService _context;
        public AdminController(IGameService context)
        {
            _context = context;
        }

        // GET: Admin
        [HttpGet()]
        public async Task<IActionResult> Index()
        {
            var response = await _context.GetGameListAsync(null, 1, 3);
            return response != null ?
                        View(response.Data) :
                        Problem("Entity set 'AppDbContext.Games'  is null.");
        }

        // GET: Admin/page
        [HttpGet("page{pageNo}")]
        public async Task<IActionResult> Index(int pageNo, int pageSize = 3)
        {
            var response = await _context.GetGameListAsync(null, pageNo, pageSize);
            return response != null ?
                        View(response.Data) :
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
        public async Task<IActionResult> Create(CreateViewModel createViewModel)
        {
            if (ModelState.IsValid)
            {
                if (createViewModel.Image != null && createViewModel.Image.Length > 0)
                {
                    await _context.CreateGameAsync(createViewModel.Game, createViewModel.Image);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(createViewModel);
        }

        // GET: AdminGames/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || (await _context.GetGameListAsync(null)).Data.Items == null)
            {
                return NotFound();
            }
            EditViewModel editViewModel = new EditViewModel();
            editViewModel.Game = (await _context.GetGameListAsync(null)).Data.Items.FirstOrDefault(m => m.Id == id);
            if (editViewModel.Game == null)
            {
                return NotFound();
            }
            return View(editViewModel);
        }

        // POST: AdminGames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditViewModel editViewModel)
        {
            if (id != editViewModel.Game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (editViewModel.Image != null && editViewModel.Image.Length > 0)
                    {
                        await _context.SaveImageAsync(id, editViewModel.Image);
                    }

                    await _context.UpdateGameAsync(id, editViewModel.Game);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await GameExists(editViewModel.Game.Id))
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
            return View(editViewModel.Game);
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
            return ((await _context.GetGameListAsync(null)).Data.Items?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
