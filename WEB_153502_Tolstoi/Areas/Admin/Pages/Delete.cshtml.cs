using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web_153502_Tolstoi.API.Data;
using Web_153502_Tolstoi.API.Services;
using Web_153502_Tolstoi.Domain.Entities;

namespace WEB_153502_Tolstoi.Areas.Admin.Pages
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IGameService _gameService;

        public DeleteModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        [BindProperty]
        public Game Game { get; set; } = default!;

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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var game = await _gameService.GetGameByIdAsync(id);

            if (game.Success != false)
            {
                Game = game.Data;
                await _gameService.DeleteGameAsync(id);
            }

            return RedirectToPage("./Index");
        }
    }
}
