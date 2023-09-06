using Microsoft.AspNetCore.Mvc;

namespace WEB_153502_Tolstoi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
