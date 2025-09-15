using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SqlSherlock.Data;
using SqlSherlock.Models;
using System.Configuration;

namespace SqlSherlock.Controllers
{
    public class QueriesController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public QueriesController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        // GET: Queries
        [HttpGet]
        public IActionResult Index()
        {
            var queryLibrary = new QueryLibrary(_environment.ContentRootPath);
            var flows = queryLibrary.GetQueryFlows();

            var environmentLibrary = new ConnectionLibrary();
            var envs = environmentLibrary.GetConnections();

            var instanceName = ConfigurationManager.AppSettings.Get("InstanceName") ?? "Sherlock";
            
            var vm = new SherlockViewModel() {
                Environments = envs,
                Flows = flows,
                InstanceName = instanceName
            };

            return Json(vm);
        }
    }
}