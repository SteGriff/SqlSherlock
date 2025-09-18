using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace SqlSherlock.Data.Tests
{
    [TestClass]
    public class InsensitiveModelTests
    {
        [TestMethod]
        public void InsensitiveModel_LowerCasesModelKeys()
        {
            // Arrange
            var storedValue = "BugsRock";
            var original = new Dictionary<string, JsonElement>()
            {
                { "MyExcellentParameter", JsonElementHelper.FromPrimitive(storedValue) }
            };

            // Act
            var target = new InsensitiveModel(original);

            // Assert
            // Key is lowercase
            var expectedKey = "myexcellentparameter";
            var actualKey = target.Model.FirstOrDefault().Key;
            Assert.AreEqual(expectedKey, actualKey);

            // Can retrieve the object by its key
            var retrieved = target.Model[expectedKey];
            Assert.AreEqual(storedValue, retrieved.ToString());
        }
    }
}
