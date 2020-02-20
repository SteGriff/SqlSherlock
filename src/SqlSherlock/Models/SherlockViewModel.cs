using SqlSherlock.Data;
using System.Collections.Generic;
using System.Linq;

namespace SqlSherlock.Models
{
    public class SherlockViewModel
    {
        public List<Connection> Environments { get; set; }

        public List<QueryFlow> Flows { get; set; }

        public bool HasFlows { get { return Flows.Count() > 1; } }
    }
}