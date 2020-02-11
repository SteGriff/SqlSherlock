using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSherlock.Data.Tests
{
    [TestClass]
    public class InsensitiveModelTests
    {
        [TestMethod]
        public void InsensitiveModel_LowerCasesModelKeys()
        {
            // Arrange
            var storedObject = new { Name = "HelloWorld" };
            var original = new Dictionary<string, object>()
            {
                { "MyExcellentParameter", storedObject }
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
            Assert.AreSame(storedObject, retrieved);
        }
    }
}
