using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSherlock.Data
{
    public static class DataTypes
    {
        public static Dictionary<string, string> InputTypeMap = new Dictionary<string, string>()
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
    }
}
