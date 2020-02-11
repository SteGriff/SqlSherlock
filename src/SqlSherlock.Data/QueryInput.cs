using System.Collections.Generic;
using System.Data;

namespace SqlSherlock.Data
{
    public class QueryInput
    {
        private Dictionary<string, string> _inputTypes = new Dictionary<string, string>()
        {
            {"bigint", "number"},
            {"binary", "text"},
            {"bit", "checkbox"},
            {"char", "text"},
            {"datetime", "datetime-local"},
            {"decimal", "number"},
            {"float", "number"},
            {"image", "text"},
            {"int", "number"},
            {"money", "number"},
            {"nchar", "text"},
            {"ntext", "text"},
            {"nvarchar", "text"},
            {"real", "number"},
            {"uniqueidentifier", "text"},
            {"smalldatetime", "datetime-local"},
            {"smallint", "number"},
            {"smallmoney", "number"},
            {"text", "text"},
            {"timestamp", "datetime-local"},
            {"tinyint", "number"},
            {"varbinary", "text"},
            {"varchar", "text"},
            {"variant", "text"},
            {"xml", "text"},
            {"udt", "text"},
            {"structured", "text"},
            {"date", "datetime-local"},
            {"time", "datetime-local"},
            {"datetime2", "datetime-local"}
        };

        public string Name
        {
            get
            {
                return SqlName.Replace("@", "");
            }
        }

        public string InputType
        {
            get
            {
                if (_inputTypes.ContainsKey(SqlDataType))
                {
                    return _inputTypes[SqlDataType];
                }
                return "text";
            }
        }

        public string SqlName { get; set; }

        public string SqlDataType { get; set; }

        public QueryInput(string sqlName, SqlDbType sqlDataType)
        {
            SqlName = sqlName;
            SqlDataType = sqlDataType.ToString().ToLower();
        }
    }
}
