using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSherlock.Data.Tests
{
    [TestClass]
    public class SqlParametersBuilderTests
    {
        [TestMethod]
        public void SqlParamsBuilder_SetsIntValue()
        {
            var target = new SqlParametersBuilder();
            const int expectedId = 54321;
            
            var query = new Query()
            {
                SqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@UserId", System.Data.SqlDbType.Int)
                },
                Inputs = new List<QueryInput>()
                {
                    new QueryInput("@UserId", System.Data.SqlDbType.Int)
                }
            };

            // For some reason, this is the shape of model data
            object inModelValue = new[] { (object)expectedId };
            var model = new Dictionary<string, object>()
            {
                {"UserId", inModelValue }
            };

            // Act
            var sqlParams = target.PopulateSqlParameters(query, model);

            // Assert
            Assert.AreEqual(1, sqlParams.Count, "There should be 1 param");
            var theParam = sqlParams.FirstOrDefault();
            Assert.IsNotNull(theParam);

            // The value has been assigned
            Assert.AreEqual(expectedId, theParam.Value);
        }
    }
}
