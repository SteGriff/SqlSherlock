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
        private string WebAppPhysicalPath { get; set; }

        private string SqlPath
        {
            get
            {
                return Path.GetDirectoryName(WebAppPhysicalPath) + @"\sql\";
            }
        }
        
        public QueryLibrary(string webAppPhysicalPath)
        {
            WebAppPhysicalPath = webAppPhysicalPath;
        }

        public List<QueryFlow> GetQueryFlows()
        {
            var queryFlows = new List<QueryFlow>();

            var foldersInSqlPath = Directory.EnumerateDirectories(SqlPath);
            if (foldersInSqlPath.Any())
            {
                // Has flows
                foreach (var path in foldersInSqlPath)
                {
                    var leafFolderName = new DirectoryInfo(path).Name;
                    var queries = GetQueriesForPath(path);
                    var flow = new QueryFlow(leafFolderName, queries);
                    queryFlows.Add(flow);
                }
            }
            else
            {
                // No flows - SQL files directly in /sql/ directory.
                // Create a "Default" flow

                var queries = GetQueriesForPath(SqlPath);
                var flow = new QueryFlow(queries);
                queryFlows.Add(flow);
            }

            return queryFlows;
        }

        public List<Query> GetQueriesForFlowName(string flowName)
        {
            string path = flowName.ToLower() == "default"
                ? SqlPath
                : Path.Combine(SqlPath, flowName);

            return GetQueriesForPath(path);
        }

        public List<Query> GetQueriesForPath(string path)
        {
            var queries = new List<Query>();
            var sqlFiles = Directory.EnumerateFiles(path, "*.sql");
            foreach (var sqlFile in sqlFiles)
            {
                var tidyFileName = Path.GetFileNameWithoutExtension(sqlFile);
                queries.Add(new Query(tidyFileName, sqlFile));
            }

            return queries;
        }
    }
}
