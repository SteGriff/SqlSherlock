using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlSherlock.Data.Tests
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void StripSqlComments_PreservesInlineComment()
        {
            string commentLine = " -- Please keep me";
            string expected = "Please keep me";

            string actual = commentLine.StripSqlCommentMarkers();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StripSqlComments_PreservesCommentAfterStartMarker()
        {
            string commentLine = "/* Hello world";
            string expected = "Hello world";

            string actual = commentLine.StripSqlCommentMarkers();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StripSqlComments_PreservesCommentBeforeEndMarker()
        {
            string commentLine = "Goodbye world */";
            string expected = "Goodbye world";

            string actual = commentLine.StripSqlCommentMarkers();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StripSqlComments_PreservesCommentInOneLineBlockComment()
        {
            string commentLine = "/* Sandwich comment */";
            string expected = "Sandwich comment";

            string actual = commentLine.StripSqlCommentMarkers();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StripSqlComments_PreservesCommentWithDoubleDashInsideBlock()
        {
            string commentLine = "/* Sandwich comment -- tasty */";
            string expected = "Sandwich comment -- tasty";

            string actual = commentLine.StripSqlCommentMarkers();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StripSqlComments_PreservesCommentWithDoubleDashInsideInline()
        {
            string commentLine = "-- Inline comment -- tasty";
            string expected = "Inline comment -- tasty";

            string actual = commentLine.StripSqlCommentMarkers();

            Assert.AreEqual(expected, actual);
        }
    }
}
