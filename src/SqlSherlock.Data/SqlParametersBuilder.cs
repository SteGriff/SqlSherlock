using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSherlock.Data
{
    public class SqlParametersBuilder
    {
        // TODO: Combine model with Query info to make list<SqlParameter>

        public List<SqlParameter> PopulateSqlParameters(Query query, Dictionary<string, object> model)
        {
            var sqlParams = query.SqlParameters;
            foreach(var sqlParam in sqlParams)
            {
                var matchedInput = query.Inputs.FirstOrDefault(i => i.SqlName == sqlParam.ParameterName);
                if (matchedInput == null) continue;

                var modelKey = matchedInput.Name.ToLower();
                if (!model.ContainsKey(modelKey)) continue;

                var matchedModelEntry = ((IEnumerable<object>)model[modelKey]).FirstOrDefault();
                sqlParam.Value = matchedModelEntry;
            }

            return sqlParams;
        }
    }
}
