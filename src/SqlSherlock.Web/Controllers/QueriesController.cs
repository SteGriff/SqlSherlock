using Microsoft.AspNetCore.Mvc;
using SqlSherlock.Data;
using SqlSherlock.Models;

namespace SqlSherlock.Web.Controllers
{
    public class QueriesController(IWebHostEnvironment environment, IConfiguration configuration) : Controller
    {
        // GET: Queries
        [HttpGet]
        public IActionResult Index()
        {
            var queryLibrary = new QueryLibrary(environment.ContentRootPath);
            var flows = queryLibrary.GetQueryFlows();

            var environmentLibrary = new ConnectionLibrary(configuration);
            var envs = environmentLibrary.GetConnections();

            var instanceName = configuration["InstanceName"] ?? "Sherlock";
            
            var vm = new SherlockViewModel() {
                Environments = envs,
                Flows = flows,
                InstanceName = instanceName
            };

            return Json(vm);
        }
    }
}