using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlSherlock.Data.Tests
{
    [TestClass]
    public class QueryInputTests
    {
        [TestMethod]
        public void QueryInput_SetsNameWithoutAtSign()
        {
            var queryInput = new QueryInput("@UserId", System.Data.SqlDbType.Int);
            Assert.AreEqual("UserId", queryInput.Name);
        }

        [TestMethod]
        public void QueryInput_SetsBasicInputTypes()
        {
            var queryInputNumeric = new QueryInput("@UserId", System.Data.SqlDbType.Int);
            Assert.AreEqual("number", queryInputNumeric.InputType);

            var queryInputText = new QueryInput("@ProductCode", System.Data.SqlDbType.NVarChar);
            Assert.AreEqual("text", queryInputText.InputType);
        }
    }
}
