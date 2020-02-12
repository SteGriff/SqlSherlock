using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlSherlock.Data.Tests
{
    [TestClass]
    public class DataTableTests
    {
        [TestMethod]
        public void DataTable_EmptyConstructor_IsSafeToUseAfterInstantiation()
        {
            var target = new DataTable();
            target.ColumnHeadings.Add("PowerLevel");
            target.Lines.Add(new object[] { 9005 });

            // No crash = good news
            // Also may as well check Error is not set
            Assert.IsNull(target.Error);
        }

        [TestMethod]
        public void DataTable_ErrorConstructor_IsSafeToUseAfterInstantiation()
        {
            var target = new DataTable("Hello", "Fail");
            target.ColumnHeadings.Add("PowerLevel");
            target.Lines.Add(new object[] { 9005 });

            // No crash = good news
        }

        [TestMethod]
        public void DataTable_ErrorConstructor_SetsFields()
        {
            const string name = "My Query";
            const string error = "It failed";
            var target = new DataTable(name, error);

            Assert.AreEqual(name, target.QueryName);
            Assert.AreEqual(error, target.Error);
        }
    }
}
