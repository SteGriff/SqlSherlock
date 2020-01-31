using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

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

        public List<string> CommentLines { get; set; }
        
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
            Init();
        }

        private void Init()
        {
            SqlParameters = new List<SqlParameter>();
            ExecutableLines = new List<string>();
            CommentLines = new List<string>();
        }

        /// <summary>
        /// Forms an ExecutableSql collection which is the SQL without DECLARE or GO statements, or comments.
        /// Puts Comments into a separate collection
        /// </summary>
        /// <param name="filePath">The file path of the SQL to process</param>
        public void ParseSql(string filePath)
        {
            Init();

            var lines = File.ReadAllLines(filePath);
            bool inCommentBlock = false;

            foreach (var line in lines)
            {
                // Ignore empty lines
                if (string.IsNullOrWhiteSpace(line)) continue;

                // Remove 'go' statements
                if (IsGo(line)) continue;

                // Preserve block comment content 
                if (inCommentBlock)
                {
                    if (EndsCommentBlock(line))
                    {
                        inCommentBlock = false;
                    }
                    SaveCommentIfNonEmpty(line);
                    continue;
                }
                else if (StartsCommentBlock(line))
                {
                    inCommentBlock = true;
                    SaveCommentIfNonEmpty(line);
                    continue;
                }
                else if (IsInlineComment(line))
                {
                    // Preserve inline comment content
                    SaveCommentIfNonEmpty(line);
                    continue;
                }

                // Parse delcarations and save meaningful SQL statements
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

        private void SaveCommentIfNonEmpty(string line)
        {
            string commentContent = line.StripSqlCommentMarkers();
            if (!string.IsNullOrEmpty(commentContent))
            {
                CommentLines.Add(commentContent);
            }
        }

        /// <summary>
        /// Naive guess at whether the line is a comment. Doesn't account for inside block comments.
        /// </summary>
        /// <param name="line">The trimmed line content of SQL to check</param>
        /// <returns>True if the line is a comment</returns>
        private bool IsInlineComment(string line)
        {
            return line.Trim().StartsWith(SqlSyntax.INLINE_COMMENT);
        }

        private bool StartsCommentBlock(string line)
        {
            return line.Trim().StartsWith(SqlSyntax.START_COMMENT);
        }

        private bool EndsCommentBlock(string line)
        {
            return line.Trim().EndsWith(SqlSyntax.END_COMMENT);
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
