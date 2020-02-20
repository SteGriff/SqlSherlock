using Newtonsoft.Json;
using System.Configuration;
using System.Web.Script.Serialization;

namespace SqlSherlock.Data
{
    public class Connection
    {
        public string Name { get; set; }

        public string ProviderName { get; set; }

        [ScriptIgnore, JsonIgnore]
        public string ConnectionString { get; set; }

        public Connection(ConnectionStringSettings connection)
        {
            Name = connection.Name;
            ProviderName = connection.ProviderName;
            ConnectionString = connection.ConnectionString;
        }
    }
}
