using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_153502_Tolstoi.API.Data;
using Web_153502_Tolstoi.API.Services;
using Web_153502_Tolstoi.Domain.Entities;

namespace WEB_153502_Tolstoi.Areas.Admin.Pages
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IGameService _gameService;

        public EditModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        [BindProperty]
        public Game Game { get; set; } = default!;
        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            Game = game.Data;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Image == null)
            {
                return Page();
            }

            try
            {
                await _gameService.UpdateGameAsync(Game.Id, Game);
                await _gameService.SaveImageAsync(Game.Id, Image);
            }
            catch (DbUpdateConcurrencyException)
            {
                if ((await _gameService.GetGameByIdAsync(Game.Id)).Success != false)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
