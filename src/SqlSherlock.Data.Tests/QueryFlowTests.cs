using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlSherlock.Data.Tests
{
    [TestClass]
    public class QueryFlowTests
    {
        [TestMethod]
        public void QueryFlow_UsesDefaultName()
        {
            var target = new QueryFlow(new List<Query>());
            Assert.AreEqual("Default", target.Name);
        }

        [TestMethod]
        public void QueryFlow_StepNumberIsZero()
        {
            var target = new QueryFlow(new List<Query>());
            Assert.AreEqual(0, target.StepNumber);
        }

        [TestMethod]
        public void QueryFlow_ConstructorSetsNameAndQueries()
        {
            const string flowName = "Product Setup";
            var target = new QueryFlow(flowName, new List<Query>());
            Assert.AreEqual(flowName, target.Name);
            Assert.AreEqual(0, target.Queries.Count);
        }
    }
}
