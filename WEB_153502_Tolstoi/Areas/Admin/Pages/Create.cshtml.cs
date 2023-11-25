using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_153502_Tolstoi.API.Data;
using Web_153502_Tolstoi.API.Services;
using Web_153502_Tolstoi.Domain.Entities;

namespace WEB_153502_Tolstoi.Areas.Admin.Pages
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IGameService _gameService;

        public CreateModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Game Game { get; set; } = default!;
        [BindProperty]
        public IFormFile? Image { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Game == null || Image == null)
            {
                return Page();
            }
            var games = (await _gameService.GetFullGameListAsync()).Data;
            List<int> indexesList = new List<int>();
            foreach (var game in games)
            {
                indexesList.Add(game.Id);
            }
            Game.Id = GetMinUniqueIndex(indexesList);
            await _gameService.CreateGameAsync(Game);

            await _gameService.SaveImageAsync(Game.Id, Image);

            return RedirectToPage("./Index");
        }

        static int GetMinUniqueIndex(List<int> existingIndexes)
        {
            int minUniqueIndex = 1;

            // Сортируем существующие индексы
            existingIndexes.Sort();

            foreach (int index in existingIndexes)
            {
                if (index > minUniqueIndex)
                {
                    // Если текущий индекс больше минимального уникального индекса, возвращаем его
                    return minUniqueIndex;
                }

                // Увеличиваем минимальный уникальный индекс на 1
                minUniqueIndex++;
            }

            // Если все индексы в списке уже заняты, возвращаем следующий индекс
            return minUniqueIndex;
        }
    }
}
