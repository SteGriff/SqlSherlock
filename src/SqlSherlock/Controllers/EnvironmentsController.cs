using Microsoft.AspNetCore.Mvc;
using SqlSherlock.Data;

namespace SqlSherlock.Controllers
{
    public class EnvironmentsController : Controller
    {
        // GET: Environments
        public IActionResult Index()
        {
            var library = new ConnectionLibrary();
            var result = library.GetConnections();

            return Json(result);
        }
    }
}