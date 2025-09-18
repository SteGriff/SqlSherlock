using Microsoft.AspNetCore.Mvc;

namespace SqlSherlock.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}