using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSherlock.Data
{
    public class Parser
    {
        private List<string> _knownDataTypes = new List<string>() {
            "int",
            "bigint",
            "smallint",
            "double",
            "float",
            "varchar",
            "nvarchar",
            "text",
            "bit"
        };

        public List<SqlParameter> SqlParameters { get; set; }

        public List<QueryInput> QueryInputs
        {
            get
            {
                return SqlParameters
                    .Select(p => new QueryInput()
                    {
                        SqlName = p.ParameterName,
                        SqlDataType = p.SqlDbType.ToString().ToLower()
                    })
                    .ToList();
            }
        }

        private List<string> ExecutableLines { get; set; }

        public string ExecutableSql
        {
            get
            {
                return string.Join(Environment.NewLine, ExecutableLines);
            }
        }

        public Parser()
        {
            SqlParameters = new List<SqlParameter>();
            ExecutableLines = new List<string>();
        }

        public void ParseSql(string fileName)
        {
            SqlParameters = new List<SqlParameter>();
            ExecutableLines = new List<string>();

            var lines = File.ReadAllLines(fileName);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (IsComment(line)) continue;

                // Remove 'go' statements
                if (IsGo(line)) continue;

                var declaration = ParseDeclaration(line);
                if (declaration != null)
                {
                    SqlParameters.Add(declaration);
                }
                else
                {
                    // This is a regular, meaningful SQL line which is not a DECLARE statement
                    ExecutableLines.Add(line);
                }
            }
        }

        /// <summary>
        /// Naive guess at whether the line is a comment. Doesn't account for inside block comments.
        /// </summary>
        /// <param name="line">The trimmed line content of SQL to check</param>
        /// <returns>True if the line is a comment</returns>
        private bool IsComment(string line)
        {
            line = line.Trim();
            return line.StartsWith("--") || line.StartsWith("/*") || line.EndsWith("*/");
        }

        private bool IsGo(string line)
        {
            return line.Trim().ToLower() == "go";
        }

        private SqlParameter ParseDeclaration(string line)
        {
            SqlParameter declaration = null;
            line = line.Trim().ToLower();

            if (!line.Contains("declare")) return null;

            // If the author assigns their own value, we don't need an input for it
            if (line.Contains("=")) return null;

            // Tokenize the line and parse for name, type, and size:
            var tokens = line.Split(new char[] { ' ', '\t', '(', ')' });

            var paramName = tokens.FirstOrDefault(t => t.StartsWith("@"));
            if (paramName == null) return null;

            var paramType = tokens.FirstOrDefault(t => _knownDataTypes.Contains(t));
            if (paramType == null) return null;

            // May have size
            var size = tokens.FirstOrDefault(t => int.TryParse(t, out int value));
            int? intSize = size == null
                ? (int?)null
                : int.Parse(size);

            // Could check null/not null here

            // We have a full parameter
            var dbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), paramType, ignoreCase: true);
            if (intSize == null)
            {
                declaration = new SqlParameter(paramName, dbType);
            }
            else
            {
                declaration = new SqlParameter(paramName, dbType, intSize.Value);
            }

            return declaration;
        }
    }
}
