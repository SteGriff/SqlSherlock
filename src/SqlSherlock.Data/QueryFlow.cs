using SqlSherlock.Data;
using System.Collections.Generic;

namespace SqlSherlock.Data
{
    public class QueryFlow
    {
        public string Name { get; set; }
        public List<Query> Queries { get; set; }
        public int StepNumber { get; }
        
        public QueryFlow(List<Query> queries)
        {
            Name = "Default";
            Queries = queries;
        }

        public QueryFlow(string name, List<Query> queries)
        {
            Name = name;
            Queries = queries;
        }
    }
}