using System.Configuration;

namespace SqlSherlock.Data.Tests
{
    public static class ConnectionFakes
    {
        public const string Name = "Test";
        public const string ConnectionString = "data source=localhost;";
        public const string Provider = "System.Data.SqlClient";

        public const string LocalSqlServer = "LocalSqlServer";
        public const string LocalConnectionString = "data source=.\\SQLEXPRESS;Integrated Security=SSPI;AttachDBFilename=|DataDirectory|aspnetdb.mdf;User Instance=true";
        
        public static ConnectionStringSettings GetFake()
        {
            return new ConnectionStringSettings(Name, ConnectionString, Provider);
        }
        public static ConnectionStringSettings GetLocalSqlServer()
        {
            return new ConnectionStringSettings(LocalSqlServer, LocalConnectionString, Provider);
        }
    }
}
