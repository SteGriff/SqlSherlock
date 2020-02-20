using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSherlock.Data.Tests
{
    [TestClass]
    public class ConnectionModelTests
    {
        [TestMethod]
        public void Connection_ConstructorSetsProperties()
        {
            var target = new Connection(ConnectionFakes.GetFake());

            Assert.AreEqual(ConnectionFakes.Name, target.Name);
            Assert.AreEqual(ConnectionFakes.ConnectionString, target.ConnectionString);
            Assert.AreEqual(ConnectionFakes.Provider, target.ProviderName);
        }
    }
}
