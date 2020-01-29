using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace SqlSherlock.Data
{
    public class DataLayer
    {
        protected string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DataConnection"].ConnectionString;
            }
        }

        private DataTable ExecuteSql(string queryName, string commandText, List<SqlParameter> sqlParameters)
        {
            var results = new DataTable();
            results.QueryName = queryName;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    // Add the parameter values
                    command.Parameters.AddRange(sqlParameters.ToArray());

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            results.ColumnHeadings = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();

                            int resultCount = 0;
                            while (reader.Read())
                            {
                                var resultLine = new object[reader.VisibleFieldCount];
                                reader.GetValues(resultLine);
                                results.Lines.Add(resultLine);
                                resultCount += 1;
                                if (resultCount > 999) break;
                            }
                        }
                    }
                }
            }

            return results;
        }

        public DataTable GetResults(Query query, List<SqlParameter> sqlParameters)
        {
            try
            {
                return ExecuteSql(query.Name, query.ExecutableSql, sqlParameters);
            }
            catch (SqlException ex)
            {
                return new DataTable(query.Name, "SQL error: " + ex.Message);
            }
            catch (Exception ex)
            {
                return new DataTable(query.Name, "General error: " + ex.Message);
            }
        }
    }

}
