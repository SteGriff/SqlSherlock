using SqlSherlock.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace SqlSherlock.Controllers
{
    public class QueryController : BaseController
    {
        /// <summary>
        /// Run a query
        /// </summary>
        /// <param name="flowName">The name of the flow (a folder of SQL files)</param>
        /// <param name="originalName">The "original" name of the query, like '0. Run stuff.sql'</param>
        /// <param name="connectionName">The DB connection string name</param>
        /// <param name="model">The model of user answers</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(
            string flowName,
            string originalName,
            string connectionName,
            Dictionary<string, object> model)
        {
            var queryLibrary = new QueryLibrary(Request.PhysicalApplicationPath);

            var queries = queryLibrary.GetQueriesForFlowName(flowName);
            var query = queries
                .FirstOrDefault(q => q.OriginalName.Trim().ToLower() == originalName.Trim().ToLower());

            if (query == null) { return new HttpStatusCodeResult(400, "No such query"); }
            
            // Get params from model + query
            var parametersBuilder = new SqlParametersBuilder();
            var sqlParameters = parametersBuilder.PopulateSqlParameters(query, model);

            // Build a DataLayer
            var connLibrary = new ConnectionLibrary();
            if (!connLibrary.HasConnectionWithName(connectionName))
            {
                return new HttpStatusCodeResult(400, "No such connection");
            }

            var dataLayer = new DataLayer(connectionName);

            var resultsTable = dataLayer.GetResults(query, sqlParameters);
            return Json(resultsTable);
        }
    }
}