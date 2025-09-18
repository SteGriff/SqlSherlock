using System.Text.Json;

namespace SqlSherlock.Web.Models
{
    /// <summary>
    /// A model class for the request to run a query
    /// </summary>
    public class QueryDataModel
    {
        public QueryDataModel() { }

        /// <summary>
        /// The name of the flow (a folder of SQL files)
        /// </summary>
        public string? FlowName { get; set; }

        /// <summary>
        /// The "original" name of the query, like '0. Run stuff.sql'
        /// </summary>
        public string? OriginalName { get; set; }

        /// <summary>
        /// The DB connection string name
        /// </summary>
        public string? ConnectionName { get; set; }

        /// <summary>
        /// The model of user answers
        /// </summary>
        public Dictionary<string, JsonElement>? Model { get; set; }
    }
}
