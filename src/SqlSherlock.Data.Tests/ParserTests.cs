using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlSherlock.Data.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void Parser_ExtractsOneDeclaration()
        {
            // Arrange
            string sql = @"declare @UserId int";
            var lines = sql.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            // Act
            var target = new Parser();
            target.ParseSql(lines);

            // Assert
            Assert.AreEqual(1, target.SqlParameters.Count, "There should be one SqlParameter");
            Assert.AreEqual("@UserId", target.SqlParameters[0].ParameterName, "The SqlParameter ParameterName should be correct");
            Assert.AreEqual(SqlDbType.Int, target.SqlParameters[0].SqlDbType, "The SqlParameter SqlDbType should be correct");

            // Negative assertions
            Assert.AreEqual(0, target.CommentLines.Count, "There should be no CommentLines");
            Assert.IsTrue(string.IsNullOrEmpty(target.ExecutableSql), "There should be no ExecutableSql");
        }

        [TestMethod]
        public void Parser_ExtractsOneDeclarationWithSize()
        {
            // Arrange
            string sql = @"declare @ProductCode nvarchar(10)";
            var lines = sql.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            // Act
            var target = new Parser();
            target.ParseSql(lines);

            // Assert
            Assert.AreEqual("@ProductCode", target.SqlParameters[0].ParameterName);
            Assert.AreEqual(SqlDbType.NVarChar, target.SqlParameters[0].SqlDbType);
            Assert.AreEqual(10, target.SqlParameters[0].Size);
        }

        [TestMethod]
        public void Parser_ExtractsMultipleDeclarations()
        {
            // Arrange
            string sql = @"declare @UserId int
declare @ProductId int
declare @ProductCode nvarchar(10)";
            var lines = sql.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            // Act
            var target = new Parser();
            target.ParseSql(lines);

            // Assert
            Assert.AreEqual(3, target.SqlParameters.Count, "There should be 3 SqlParameters");

            Assert.AreEqual("@UserId", target.SqlParameters[0].ParameterName);
            Assert.AreEqual(SqlDbType.Int, target.SqlParameters[0].SqlDbType);

            Assert.AreEqual("@ProductId", target.SqlParameters[1].ParameterName);
            Assert.AreEqual(SqlDbType.Int, target.SqlParameters[1].SqlDbType);

            Assert.AreEqual("@ProductCode", target.SqlParameters[2].ParameterName);
            Assert.AreEqual(SqlDbType.NVarChar, target.SqlParameters[2].SqlDbType);
            Assert.AreEqual(10, target.SqlParameters[2].Size);
        }

        [TestMethod]
        public void Parser_RemovesGo()
        {
            // Arrange
            string sql = @"declare @UserId int
select * from Table
go
select * from AnotherTable
go";
            var lines = sql.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            // Act
            var target = new Parser();
            target.ParseSql(lines);

            // Assert
            var executableLines = target.ExecutableSql.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Assert.AreEqual(2, executableLines.Length, "There should be 2 executable lines only");
            foreach(var line in executableLines)
            {
                Assert.AreNotEqual("go", line.ToLower(), "No line should be 'go'");
            }
        }

        [TestMethod]
        public void Parser_RemovesLinesThatAreWhitespaceOnly()
        {
            // Arrange
            // Second line is 4 spaces
            string sql = @"select * from Table
    
select * from AnotherTable";

            var lines = sql.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            // Act
            var target = new Parser();
            target.ParseSql(lines);

            // Assert
            var executableLines = target.ExecutableSql.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Assert.AreEqual(2, executableLines.Length, "There should be 2 executable lines only");
            foreach (var line in executableLines)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(line));
            }
        }

        [TestMethod]
        public void Parser_ExtractsInlineComment()
        {
            // Arrange
            string sql = @"-- Cool query
declare @UserId int";
            var lines = sql.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            // Act
            var target = new Parser();
            target.ParseSql(lines);

            // Assert
            Assert.AreEqual(1, target.CommentLines.Count, "There should be one CommentLine");
            Assert.AreEqual("Cool query", target.CommentLines[0], "The CommentLine should be as correct and trimmed");
        }

        [TestMethod]
        public void Parser_ExtractsBlockComment()
        {
            // Arrange
            string sql = @"/* Now this is a story
all about how
my life got flipped
turned upside-down */
declare @UserId int";
            var lines = sql.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            // Act
            var target = new Parser();
            target.ParseSql(lines);

            // Assert
            Assert.AreEqual(4, target.CommentLines.Count, "There should be 4 CommentLines");
            Assert.AreEqual("Now this is a story",  target.CommentLines[0]);
            Assert.AreEqual("all about how",        target.CommentLines[1]);
            Assert.AreEqual("my life got flipped",  target.CommentLines[2]);
            Assert.AreEqual("turned upside-down",   target.CommentLines[3]);
        }

        [TestMethod]
        public void Parser_ExtractsAllParametersCommentsAndExecutableSql()
        {
            // Arrange
            string sql = @"/* Check the users */
declare @UserId int
declare @Height int

-- Does the user exist?

select * from Users
where UserId = @UserId
and Height = @Height";
            var lines = sql.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            // Act
            var target = new Parser();
            target.ParseSql(lines);

            // Assert
            // Comments
            Assert.AreEqual(2, target.CommentLines.Count, "There should be 2 CommentLines");
            Assert.AreEqual("Check the users", target.CommentLines[0]);
            Assert.AreEqual("Does the user exist?", target.CommentLines[1]);

            // Executable SQL
            var executableLines = target.ExecutableSql.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Assert.AreEqual(3, executableLines.Length, "There should be 3 executableLines");
            CollectionAssert.AllItemsAreNotNull(executableLines);
            CollectionAssert.AllItemsAreUnique(executableLines);

            // Params
            Assert.AreEqual(2, target.SqlParameters.Count, "There should be 2 SqlParameters");
            CollectionAssert.AllItemsAreNotNull(target.SqlParameters);
            CollectionAssert.AllItemsAreUnique(target.SqlParameters);
        }
    }
}
