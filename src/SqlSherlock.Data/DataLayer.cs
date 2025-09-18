using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlSherlock.Data
{
    public class DataLayer
    {
        private readonly string _connectionStringName;
        private readonly IConfiguration _configuration;

        public DataLayer(IConfiguration configuration)
        {
            const string defaultConnectionString = "DataConnection";
            _connectionStringName = defaultConnectionString;
            _configuration = configuration;
        }

        public DataLayer(string connectionStringName, IConfiguration configuration)
        {
            _connectionStringName = connectionStringName;
            _configuration = configuration;
        }

        protected string ConnectionString => _configuration.GetConnectionString(_connectionStringName);

        private DataTable ExecuteSql(string queryName, string commandText, List<SqlParameter> sqlParameters)
        {
            var results = new DataTable
            {
                QueryName = queryName
            };

            using SqlConnection connection = new(ConnectionString);
            connection.Open();

            using SqlCommand command = new(commandText, connection);
            command.Parameters.AddRange([.. sqlParameters]);

            using var reader = command.ExecuteReader();

            if (reader == null)
                return results;

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
