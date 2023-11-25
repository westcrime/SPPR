using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_153502_Tolstoi.API.Services;
using Web_153502_Tolstoi.Domain.Entities;

namespace WEB_153502_Tolstoi.Controllers
{
    public class CartController : Controller
    {
        private readonly IGameService _gameService;
        private readonly Cart _cart;
        public CartController(IGameService bookService, Cart cart)
        {
            _gameService = bookService;
            _cart = cart;
        }


        public IActionResult Index()
        {
            return View(_cart.CartItems);
        }

        [Authorize]
        [Route("[controller]/add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnurl)
        {
            var data = await _gameService.GetGameByIdAsync(id);
            if (data.Success)
            {
                _cart.AddToCart(data.Data);
            }

            return Redirect(returnurl);
        }
        [Authorize]
        public IActionResult Remove(int id)
        {
            _cart.RemoveItems(id);
            return View("Index", _cart.CartItems);
        }
        [Authorize]
        public IActionResult Clear()
        {
            _cart.ClearAll();
            return View("Index", _cart.CartItems);
        }
    }
}
