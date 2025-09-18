using System.Text.Json.Serialization;

namespace SqlSherlock.Data
{
    public class Connection(string name, string connectionString)
    {
        public string Name { get; set; } = name;

        [JsonIgnore]
        public string ConnectionString { get; set; } = connectionString;
    }
}
