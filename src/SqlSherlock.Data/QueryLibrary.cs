using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSherlock.Data
{
    public class QueryLibrary
    {
        public QueryLibrary()
        {

        }

        public List<Query> GetQueries(string webAppPhysicalPath)
        {
            var queries = new List<Query>();

            var sqlPath = Path.GetDirectoryName(webAppPhysicalPath) + @"\sql\";
            var sqlFiles = Directory.EnumerateFiles(sqlPath, "*.sql");
            foreach (var sqlFile in sqlFiles)
            {
                var tidyFileName = Path.GetFileNameWithoutExtension(sqlFile);
                queries.Add(new Query(tidyFileName, sqlFile));
            }

            return queries;
        }
    }
}
