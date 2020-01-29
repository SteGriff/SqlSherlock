using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace SqlSherlock.Data
{
    public class Query
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public string OriginalName { get; set; }
        public string FilePath { get; set; }

        /// <summary>
        /// The SQL of the file, minus DECLARE statements which would conflict with
        /// user-provided values for SqlParameters
        /// </summary>
        [ScriptIgnore]
        public string ExecutableSql { get; set; }

        [ScriptIgnore]
        public List<SqlParameter> SqlParameters { get; set; }

        public List<QueryInput> Inputs { get; set; }

        public DataTable Result { get; set; }

        public Query(string name, string file)
        {
            OriginalName = name.Trim();

            try
            {
                Name = Regex.Replace(OriginalName, @"[0-9]+\.", "").Trim();
            }
            catch (Exception)
            {
                Name = OriginalName;
            }

            try
            {
                var numString = OriginalName.Substring(0, OriginalName.IndexOf('.'));
                Number = int.Parse(numString);
            }
            catch (Exception)
            {
                Number = 0;
            }

            FilePath = file;

            var parser = new Parser();
            parser.ParseSql(FilePath);

            ExecutableSql = parser.ExecutableSql;
            SqlParameters = parser.SqlParameters;
            Inputs = parser.QueryInputs;
        }

    }
}
