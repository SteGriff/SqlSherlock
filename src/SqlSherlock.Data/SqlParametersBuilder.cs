using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SqlSherlock.Data
{
    public class SqlParametersBuilder
    {
        /// <summary>
        /// Combine model with Query info to make list<SqlParameter>
        /// </summary>
        /// <param name="query">The Query, pulled from QueryLibrary</param>
        /// <param name="model">The user's submitted data model from web</param>
        /// <returns></returns>
        public List<SqlParameter> PopulateSqlParameters(Query query, Dictionary<string, object> model)
        {
            var caseInsensitiveModel = new InsensitiveModel(model).Model;

            var sqlParams = query.SqlParameters;
            foreach(var sqlParam in sqlParams)
            {
                var matchedInput = query.Inputs.FirstOrDefault(i => i.SqlName == sqlParam.ParameterName);
                if (matchedInput == null) continue;

                var modelKey = matchedInput.Name.ToLower();
                if (!caseInsensitiveModel.ContainsKey(modelKey)) continue;

                // For some reason, model entries are IEnumerable<object> containing one entry, the piece of data we wanted
                var matchedModelEntry = ((IEnumerable<object>)caseInsensitiveModel[modelKey]).FirstOrDefault();
                sqlParam.Value = matchedModelEntry;
            }

            return sqlParams;
        }
    }
}
