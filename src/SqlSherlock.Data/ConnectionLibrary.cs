using System.Collections.Generic;
using System.Configuration;

namespace SqlSherlock.Data
{
    public class ConnectionLibrary
    {
        public List<Connection> GetConnections()
        {
            var result = new List<Connection>();

            foreach (ConnectionStringSettings connection in ConfigurationManager.ConnectionStrings)
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
