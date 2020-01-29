using SqlSherlock.Data;
using SqlSherlock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SqlSherlock.Controllers
{
    public class QueriesController : Controller
    {
        // GET: Queries
        [HttpGet]
        public ActionResult Index()
        {
            // TODO Dependency Injection
            var queryLibrary = new QueryLibrary();
            var queries = queryLibrary.GetQueries(Request.PhysicalApplicationPath);

            var vm = new SherlockViewModel() { Queries = queries };

            return Json(vm, JsonRequestBehavior.AllowGet);
        }
    }
}