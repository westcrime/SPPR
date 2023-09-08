using Microsoft.AspNetCore.Mvc;
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
            item1.Id = 2;
            item1.Name = "Item 2";

            ListDemo item3 = new ListDemo();
            item1.Id = 3;
            item1.Name = "Item 3";

            List<ListDemo> list = new List<ListDemo>();
            list.Add(item1);
            list.Add(item2);
            list.Add(item3);

            return View(list);
        }
    }
}
