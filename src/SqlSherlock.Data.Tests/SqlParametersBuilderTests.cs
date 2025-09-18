using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
                SqlParameters = [
                    new("@UserId", System.Data.SqlDbType.Int)
                ],
                Inputs = [
                    new("@UserId", System.Data.SqlDbType.Int)
                ]
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

        [TestMethod]
        public void SqlParamsBuilder_SetsMultipleValues()
        {
            var expectedId = 54321M;
            var allowed = true;
            var activatedOn = DateTime.Now.ToString("o"); // ISO 8601 format

            var query = new Query()
            {
                SqlParameters = [
                    new("@UserId", System.Data.SqlDbType.Int),
                    new("@Allowed", System.Data.SqlDbType.Bit),
                    new("@ActivatedOn", System.Data.SqlDbType.DateTime2),
                ],
                Inputs = [
                    new("@UserId", System.Data.SqlDbType.Int),
                    new("@Allowed", System.Data.SqlDbType.Bit),
                    new("@ActivatedOn", System.Data.SqlDbType.DateTime2),
                ]
            };

            var model = new Dictionary<string, JsonElement>()
            {
                {"UserId", JsonElementHelper.FromPrimitive(expectedId) },
                {"Allowed", JsonElementHelper.FromPrimitive(allowed) },
                {"ActivatedOn", JsonElementHelper.FromPrimitive(activatedOn) },
            };

            // Act
            var sqlParams = SqlParametersBuilder.PopulateSqlParameters(query, model);

            // Assert
            Assert.AreEqual(3, sqlParams.Count, "There should be 3 params");

            var userIdParam = sqlParams.Where(x => x.ParameterName == "@UserId").FirstOrDefault();
            Assert.IsNotNull(userIdParam);

            var allowedParam = sqlParams.Where(x => x.ParameterName == "@Allowed").FirstOrDefault();
            Assert.IsNotNull(allowedParam);

            var activatedOnParam = sqlParams.Where(x => x.ParameterName == "@ActivatedOn").FirstOrDefault();
            Assert.IsNotNull(activatedOnParam);

            // Assert values are as assigned
            Assert.AreEqual(expectedId, userIdParam.Value);
            Assert.AreEqual(allowed, allowedParam.Value);
            Assert.AreEqual(activatedOn, activatedOnParam.Value);
        }
    }
}
