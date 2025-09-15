using Microsoft.AspNetCore.Mvc;

namespace SqlSherlock.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}