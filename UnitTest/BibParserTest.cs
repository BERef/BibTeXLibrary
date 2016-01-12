using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BibTeXLibrary;

namespace UnitTest
{
    [TestClass]
    public class BibParserTest
    {
        [TestMethod]
        public void TestParserRegularBibEntry()
        {
            var parser = new BibParser(
                new StringReader("@article{keyword, title = {\"0\"{123}456{789}}, year = 2012, address=\"PingLeYuan\"}"));
            var entry = parser.GetAllResult()[0];

            Assert.AreEqual("Article"           , entry.Type);
            Assert.AreEqual("\"0\"{123}456{789}", entry.Title);
            Assert.AreEqual("2012"              , entry.Year);
            Assert.AreEqual("PingLeYuan"        , entry.Address);
        }

        [TestMethod]
        public void TestParserString()
        {
            var parser = new BibParser(
                new StringReader("@article{keyword, title = \"hello \\\"world\\\"\", address=\"Ping\" # \"Le\" # \"Yuan\",}"));
            var entry = parser.GetAllResult()[0];

            Assert.AreEqual("Article"            , entry.Type);
            Assert.AreEqual("hello \\\"world\\\"", entry.Title);
            Assert.AreEqual("PingLeYuan"         , entry.Address);
        }

        [TestMethod]
        public void TestParserWithoutKey()
        {
            var parser = new BibParser(
                            new StringReader("@book{, title = {}}"));
            var entry = parser.GetAllResult()[0];

            Assert.AreEqual("Book", entry.Type);
            Assert.AreEqual(""    , entry.Title);
        }

        [TestMethod]
        public void TestParserWithoutKeyAndTags()
        {
            var parser = new BibParser(
                            new StringReader("@book{}"));
            var entry = parser.GetAllResult()[0];

            Assert.AreEqual("Book", entry.Type);
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedTokenException))]
        public void TestParserWithBorkenBibEntry()
        {
            var parser = new BibParser(
                            new StringReader("@book{,"));
            var entry = parser.GetAllResult()[0];
        }
    }
}
