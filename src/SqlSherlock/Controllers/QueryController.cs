using SqlSherlock.Data;
using SqlSherlock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SqlSherlock.Controllers
{
    public class QueryController : Controller
    {
        [HttpPost]
        public ActionResult Index(int step, string originalName, Dictionary<string, object> model)
        {
            var library = new QueryLibrary();
            var queries = library.GetQueries(Request.PhysicalApplicationPath);

            var query = queries.FirstOrDefault(q => q.OriginalName.Trim().ToLower() == originalName.Trim().ToLower());
            if (query == null) { return new HttpStatusCodeResult(400, "No such query"); }

            // Get params from model + query
            var parametersBuilder = new SqlParametersBuilder();
            var sqlParameters = parametersBuilder.PopulateSqlParameters(query, model);

            var dataLayer = new DataLayer();
            var resultsTable = dataLayer.GetResults(query, sqlParameters);

            return Json(resultsTable);
        }
    }
}