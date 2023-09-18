using Microsoft.AspNetCore.Mvc;
using Web_153502_Tolstoi.Domain.Entities;
using Web_153502_Tolstoi.Domain.Models;
using WEB_153502_Tolstoi.Services.CategoryServices;
using WEB_153502_Tolstoi.Services.GameService;

namespace WEB_153502_Tolstoi.Controllers
{
    public class GameController : Controller
    { 
        IGameService _gameService;
        ICategoryService _categoryService;
        string category;

        public GameController(IGameService gameService, ICategoryService catService)
        {
            _gameService = gameService;
            _categoryService = catService;
        }

        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {

            var gameResponse = new ResponseData<ListModel<Game>>();

            var categories = (await _categoryService.GetCategoryListAsync()).Data;
            ViewBag.Categories = categories;

            if (category != null) 
            {
                gameResponse = await _gameService.GetGameListAsync(category, pageNo);
                if (!gameResponse.Success)
                    return NotFound(gameResponse.ErrorMessage);

                ViewData["currentCategory"] = category;
                return View(gameResponse.Data);
                //return View(gameResponse.Data.Items.Where(game => game.CategoryId == categories.Find(cat => cat.NormalizedName == category).Id));
            }

            gameResponse = await _gameService.GetGameListAsync(null, pageNo);
            if (!gameResponse.Success)
                return NotFound(gameResponse.ErrorMessage);

            return View(gameResponse.Data);
        }
    }
}
