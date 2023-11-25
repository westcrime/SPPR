using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Web_153502_Tolstoi.Domain.Entities;

namespace WEB_153502_Tolstoi.Components
{
    [ViewComponent]
    public class CartViewComponent
    {
        private readonly Cart _cart;
        public CartViewComponent(Cart cart)
        {
            _cart = cart;
        }

        public HtmlString Invoke()
        {
            double price = _cart.TotalCost;
            int amount = _cart.Count;
            return new HtmlString($"{price} $ <i class=\"fa-solid fa-cart-shopping\"></i>({amount})");

        }
    }
}
