using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BibTeXLibrary;
using System.Text;

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

            parser.Dispose();
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

            parser.Dispose();
        }

        [TestMethod]
        public void TestParserWithoutKey()
        {
            var parser = new BibParser(
                            new StringReader("@book{, title = {}}"));
            var entry = parser.GetAllResult()[0];

            Assert.AreEqual("Book", entry.Type);
            Assert.AreEqual(""    , entry.Title);

            parser.Dispose();
        }

        [TestMethod]
        public void TestParserWithoutKeyAndTags()
        {
            var parser = new BibParser(
                            new StringReader("@book{}"));
            var entry = parser.GetAllResult()[0];

            Assert.AreEqual("Book", entry.Type);

            parser.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedTokenException))]
        public void TestParserWithBorkenBibEntry()
        {
            using (var parser = new BibParser(
                            new StringReader("@book{,")))
            {
                var entry = parser.GetAllResult()[0];
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedTokenException))]
        public void TestParserWithIncompletedTag()
        {
            using (var parser = new BibParser(
                            new StringReader("@book{,title=,}")))
            {
                var entry = parser.GetAllResult()[0];
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedTokenException))]
        public void TestParserWithBrokenTag()
        {
            using (var parser = new BibParser(
                            new StringReader("@book{,titl")))
            {
                var entry = parser.GetAllResult()[0];
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedTokenException))]
        public void TestParserWithBrokenNumber()
        {
            using (var parser = new BibParser(
                            new StringReader("@book{,title = 2014")))
            {
                var entry = parser.GetAllResult()[0];
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UnrecognizableCharacterException))]
        public void TestParserWithUnexpectedCharacter()
        {
            using (var parser = new BibParser(
                            new StringReader("@book{,ti?le = {Hadoop}}")))
            {
                var entry = parser.GetAllResult()[0];
            }
        }

        [TestMethod]
        public void TestParserWithBibFile()
        {
            var parser = new BibParser(
                            new StreamReader("TestData.bib", Encoding.Default));
            var entrys = parser.GetAllResult();

            Assert.AreEqual(3                                                    , entrys.Count);
            Assert.AreEqual("nobody"                                             , entrys[0].Publisher);
            Assert.AreEqual("Apache hadoop yarn: Yet another resource negotiator", entrys[1].Title);
            Assert.AreEqual("KalavriShang-797"                                   , entrys[2].Key);
            parser.Dispose();
        }

        [TestMethod]
        public void TestStaticParseWithBibFile()
        {
            var entrys = BibParser.Parse(new StreamReader("TestData.bib", Encoding.Default));

            Assert.AreEqual(3, entrys.Count);
            Assert.AreEqual("nobody", entrys[0].Publisher);
            Assert.AreEqual("Apache hadoop yarn: Yet another resource negotiator", entrys[1].Title);
            Assert.AreEqual("KalavriShang-797", entrys[2].Key);
        }
    }
}
