using Microsoft.AspNetCore.Mvc;
using Web_153502_Tolstoi.API.Services;
using Web_153502_Tolstoi.Domain.Entities;
using Web_153502_Tolstoi.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;
using System.Drawing.Printing;
using WEB_153502_Tolstoi.Extensions;

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

        [Route("Catalog/{category=all}/page{pageNo=1}/{pageSize=3}")]
        public async Task<IActionResult> Index(string? category = "all", int pageNo = 1, int pageSize = 3)
        {

            var gameResponse = new ResponseData<ListModel<Game>>();

            var categoryResponse = await _categoryService.GetCategoryListAsync();
            if (categoryResponse.Success != true) 
            {
                return NotFound(categoryResponse.ErrorMessage);
            }

            ViewBag.Categories = categoryResponse.Data.Items.ToList();
            gameResponse = await _gameService.GetGameListAsync(category, pageNo, pageSize);
            if (!gameResponse.Success)
                return NotFound(gameResponse.ErrorMessage);

            ViewData["currentCategory"] = category;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_GamePartial", gameResponse.Data);
            }

            return View(gameResponse.Data);
        }
    }
}
