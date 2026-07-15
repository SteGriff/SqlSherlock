using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace SqlSherlock.Data
{
    public static class SqlParametersBuilder
    {
        /// <summary>
        /// Combine model with Query info to make list<SqlParameter>
        /// </summary>
        /// <param name="query">The Query, pulled from QueryLibrary</param>
        /// <param name="model">The user's submitted data model from web</param>
        /// <returns></returns>
        public static List<SqlParameter> PopulateSqlParameters(Query query, Dictionary<string, JsonElement> model)
        {
            var caseInsensitiveModel = new InsensitiveModel(model).Model;

            var sqlParams = query.SqlParameters;
            foreach(var sqlParam in sqlParams)
            {
                var matchedInput = query.Inputs.FirstOrDefault(i => i.SqlName == sqlParam.ParameterName);
                if (matchedInput == null) continue;

                var modelKey = matchedInput.Name.ToLower();
                if (!caseInsensitiveModel.ContainsKey(modelKey)) continue;

                var matchedModelEntry = caseInsensitiveModel[modelKey];
                sqlParam.Value = matchedModelEntry ?? DBNull.Value;
            }

            return sqlParams;
        }
    }
}
