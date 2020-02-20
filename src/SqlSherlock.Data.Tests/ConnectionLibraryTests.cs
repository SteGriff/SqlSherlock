using System;
using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlSherlock.Data.Tests
{
    [TestClass]
    public class ConnectionLibraryTests
    {
        [TestMethod]
        public void ConnectionLibrary_IgnoresLocalSqlConnection()
        {
            var dummy = new ConnectionStringSettingsCollection();
            dummy.Add(ConnectionFakes.GetLocalSqlServer());
            dummy.Add(ConnectionFakes.GetFake());

            var target = new ConnectionLibrary(dummy);

            var result = target.GetConnections();

            Assert.AreEqual(1, result.Count);
            CollectionAssert.AllItemsAreNotNull(result);
            Assert.AreEqual(ConnectionFakes.Name, result.FirstOrDefault().Name);
        }
    }
}
