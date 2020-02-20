using SqlSherlock.Data;
using SqlSherlock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            //var queries = queryLibrary.GetQueries(Request.PhysicalApplicationPath);

            var flows = queryLibrary.GetQueryFlows();
            var vm = new SherlockViewModel() { Flows = flows };

            return Json(vm, JsonRequestBehavior.AllowGet);
        }
    }
}