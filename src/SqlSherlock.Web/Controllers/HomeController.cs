using Microsoft.AspNetCore.Mvc;

namespace SqlSherlock.Web.Controllers
{
    public class HomeController(IConfiguration configuration) : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = configuration["InstanceName"] ?? "Sherlock";
            ViewBag.Note = configuration["Note"];
            return View();
        }
    }
}