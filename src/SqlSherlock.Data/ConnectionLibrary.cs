﻿using System.Collections.Generic;
using System.Configuration;
using System.Linq;

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

        public bool HasConnectionWithName(string connectionName)
        {
            if (string.IsNullOrEmpty(connectionName)) return false;
            connectionName = connectionName.ToLower();
            var names = GetConnections().Select(c => c.Name.ToLower());
            return names.Any(c => c == connectionName);
        }
    }
}
