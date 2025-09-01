using Newtonsoft.Json;
using System.Configuration;

namespace SqlSherlock.Data
{
    public class Connection
    {
        public string Name { get; set; }

        public string ProviderName { get; set; }

        [JsonIgnore]
        public string ConnectionString { get; set; }

        public Connection(ConnectionStringSettings connection)
        {
            Name = connection.Name;
            ProviderName = connection.ProviderName;
            ConnectionString = connection.ConnectionString;
        }
    }
}
