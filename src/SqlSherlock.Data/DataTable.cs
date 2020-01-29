using System.Collections.Generic;

namespace SqlSherlock.Data
{
    public class DataTable
    {
        public string QueryName { get; set; }
        public List<string> ColumnHeadings { get; set; }
        public List<object[]> Lines { get; set; }
        public string Error { get; set; }

        public DataTable()
        {
            QueryName = "Untitled Query";
            ColumnHeadings = new List<string>();
            Lines = new List<object[]>();
            Error = null;
        }

        public DataTable(string queryName, string errorMessage)
        {
            QueryName = queryName;
            ColumnHeadings = new List<string>();
            Lines = new List<object[]>();
            Error = errorMessage;
        }

    }
}
