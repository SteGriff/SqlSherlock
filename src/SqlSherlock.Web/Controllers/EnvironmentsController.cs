using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SqlSherlock.Data;

namespace SqlSherlock.Web.Controllers
{
    public class EnvironmentsController(IConfiguration configuration) : Controller
    {
        // GET: Environments
        public IActionResult Index()
        {
            var library = new ConnectionLibrary(configuration);
            var connections = library.GetConnections();
            return Json(connections);
        }
    }
}