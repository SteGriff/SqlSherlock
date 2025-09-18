using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;

namespace SqlSherlock.Data.Tests
{
    [TestClass]
    public class SqlParametersBuilderTests
    {
        [TestMethod]
        public void SqlParamsBuilder_SetsIntValue()
        {
            int expectedId = 54321;
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

            var model = new Dictionary<string, JsonElement>()
            {
                {"UserId", JsonElementHelper.FromPrimitive(expectedId) }
            };

            // Act
            var sqlParams = SqlParametersBuilder.PopulateSqlParameters(query, model);

            // Assert
            Assert.AreEqual(1, sqlParams.Count, "There should be 1 param");
            var theParam = sqlParams.FirstOrDefault();
            Assert.IsNotNull(theParam);

            // The value has been assigned
            StringAssert.Equals(expectedId, theParam.Value);
        }
    }
}
