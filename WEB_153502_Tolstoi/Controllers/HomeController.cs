using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_153502_Tolstoi.Models;

namespace WEB_153502_Tolstoi.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            ViewData["Title"] = "Лабораторная работа 2";

            ListDemo item1 = new ListDemo();
            item1.Id = 1;
            item1.Name = "Item 1";

            ListDemo item2 = new ListDemo();
            item2.Id = 2;
            item2.Name = "Item 2";

            ListDemo item3 = new ListDemo();
            item3.Id = 3;
            item3.Name = "Item 3";

            List<ListDemo> list = new List<ListDemo>
            {
                item1,
                item2,
                item3
            };

            var MyList = new SelectList(list, nameof(ListDemo.Id), nameof(ListDemo.Name));

            return View(MyList);
        }
    }
}
