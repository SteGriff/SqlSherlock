using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlSherlock.Data.Tests
{
    [TestClass]
    public class QueryLibraryTests
    {
        [TestMethod]
        public void QueryLibrary_AcceptsPath()
        {
            const string webAppPath = @"C:\inetpub\www\SqlSherlock";
            var target = new QueryLibrary(webAppPath);
        }
    }
}
