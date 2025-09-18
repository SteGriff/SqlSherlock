using Microsoft.AspNetCore.Mvc;
using SqlSherlock.Data;
using SqlSherlock.Web.Models;

namespace SqlSherlock.Web.Controllers
{
    public class QueryController(IWebHostEnvironment environment, IConfiguration configuration) : Controller
    {
        /// <summary>
        /// Run a query
        /// </summary>
        /// <param name="data">The <c>QueryData</c> to use for the query run</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index([FromBody] QueryDataModel data)
        {
            var queryLibrary = new QueryLibrary(environment.ContentRootPath);

            var queries = queryLibrary.GetQueriesForFlowName(data.FlowName);
            var query = queries
                .FirstOrDefault(q => q.OriginalName.Trim().Equals(data.OriginalName.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (query == null) { return BadRequest("No such query"); }

            // Get params from model + query
            var sqlParameters = SqlParametersBuilder.PopulateSqlParameters(query, data.Model);

            // Build a DataLayer
            var connLibrary = new ConnectionLibrary(configuration);
            if (!connLibrary.HasConnectionWithName(data.ConnectionName))
            {
                return BadRequest("No such connection");
            }

            var dataLayer = new DataLayer(data.ConnectionName, configuration);

            var resultsTable = dataLayer.GetResults(query, sqlParameters);
            return Json(resultsTable);
        }
    }
}