using System.Collections.Generic;
using System.Data;

namespace SqlSherlock.Data
{
    public class QueryInput
    {
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
                if (DataTypes.InputTypeMap.ContainsKey(SqlDataType))
                {
                    return DataTypes.InputTypeMap[SqlDataType];
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
