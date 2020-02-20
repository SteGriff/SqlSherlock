using SqlSherlock.Data;
using SqlSherlock.Models;
using System.Web.Mvc;

namespace SqlSherlock.Controllers
{
    public class QueriesController : BaseController
    {
        // GET: Queries
        [HttpGet]
        public ActionResult Index()
        {
            // TODO Dependency Injection
            var queryLibrary = new QueryLibrary(Request.PhysicalApplicationPath);
            var flows = queryLibrary.GetQueryFlows();

            var environmentLibrary = new ConnectionLibrary();
            var envs = environmentLibrary.GetConnections();
            
            var vm = new SherlockViewModel() {
                Environments = envs,
                Flows = flows
            };

            return Json(vm, JsonRequestBehavior.AllowGet);
        }
    }
}