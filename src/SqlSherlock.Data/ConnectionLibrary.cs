using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace SqlSherlock.Data
{
    public class ConnectionLibrary(IConfiguration configuration)
    {
        public List<Connection> GetConnections()
        {
            var result = new List<Connection>();

            var connectionStrings = configuration.GetSection("ConnectionStrings").GetChildren();
            foreach (var connection in connectionStrings)
            {
                if (connection.Key == "LocalSqlServer")
                    continue;

                var model = new Connection(connection.Key, connection.Value);
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
