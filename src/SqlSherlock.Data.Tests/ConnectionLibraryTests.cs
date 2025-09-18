using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlSherlock.Data.Tests
{
    [TestClass]
    public class ConnectionLibraryTests
    {
        [TestMethod]
        public void ConnectionLibrary_IgnoresLocalSqlConnection()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"ConnectionStrings:LocalSqlServer", "Server=localhost;Database=master;Trusted_Connection=True;"},
                {"ConnectionStrings:FakeConnection", "Server=fake;Database=fakeDb;Trusted_Connection=True;"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var target = new ConnectionLibrary(configuration);

            var result = target.GetConnections();

            Assert.AreEqual(1, result.Count);
            CollectionAssert.AllItemsAreNotNull(result);
            Assert.AreEqual("FakeConnection", result.FirstOrDefault().Name);
        }
    }
}
