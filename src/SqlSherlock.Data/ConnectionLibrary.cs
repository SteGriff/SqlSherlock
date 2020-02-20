using System.Collections.Generic;
using System.Configuration;

namespace SqlSherlock.Data
{
    public class ConnectionLibrary
    {
        private ConnectionStringSettingsCollection _connectionStringSettingsCollection;

        public ConnectionLibrary()
        {
            _connectionStringSettingsCollection = ConfigurationManager.ConnectionStrings;
        }

        public ConnectionLibrary(ConnectionStringSettingsCollection connectionStringSettingsCollection)
        {
            _connectionStringSettingsCollection = connectionStringSettingsCollection;
        }

        public List<Connection> GetConnections()
        {
            var result = new List<Connection>();

            foreach (ConnectionStringSettings connection in _connectionStringSettingsCollection)
            {
                if (connection.Name == "LocalSqlServer")
                    continue;

                var model = new Connection(connection);

                result.Add(model);
            }

            return result;
        }
    }
}
