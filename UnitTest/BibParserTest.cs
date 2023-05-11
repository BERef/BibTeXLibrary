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
			BibParser parser = new BibParser(new StringReader("@Article{keyword, title = {\"0\"{123}456{789}}, year = 2012, address=\"PingLeYuan\"}"));
            var entry = parser.GetAllResults().Item2[0];

            Assert.AreEqual("Article"           , entry.Type);
            Assert.AreEqual("\"0\"{123}456{789}", entry.Title);
            Assert.AreEqual("2012"              , entry.Year);
            Assert.AreEqual("PingLeYuan"        , entry.Address);

            parser.Dispose();
        }

        [TestMethod]
        public void TestParserString()
        {
			BibParser parser = new BibParser(new StringReader("@article{keyword, title = \"hello \\\"world\\\"\", address=\"Ping\" # \"Le\" # \"Yuan\",}"));
            var entry = parser.GetAllResults().Item2[0];

            Assert.AreEqual("article"            , entry.Type);
            Assert.AreEqual("hello \\\"world\\\"", entry.Title);
            Assert.AreEqual("PingLeYuan"         , entry.Address);

            parser.Dispose();
        }

        [TestMethod]
        public void TestParserWithoutKey()
        {
			BibParser parser = new BibParser(new StringReader("@book{, title = {}}"));
            var entry = parser.GetAllResults().Item2[0];

            Assert.AreEqual("book", entry.Type);
            Assert.AreEqual(""    , entry.Title);

            parser.Dispose();
        }

        [TestMethod]
        public void TestParserWithoutKeyAndTags()
        {
			BibParser parser = new BibParser(new StringReader("@book{}"));
            var entry = parser.GetAllResults().Item2[0];

            Assert.AreEqual("book", entry.Type);

            parser.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedTokenException))]
        public void TestParserWithBorkenBibEntry()
        {
            using (BibParser parser = new BibParser(new StringReader("@book{,")))
            {
                parser.GetAllResults();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedTokenException))]
        public void TestParserWithIncompletedTag()
        {
            using (BibParser parser = new BibParser(new StringReader("@book{,title=,}")))
            {
                parser.GetAllResults();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedTokenException))]
        public void TestParserWithBrokenTag()
        {
            using (BibParser parser = new BibParser(new StringReader("@book{,titl")))
            {
                parser.GetAllResults();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedTokenException))]
        public void TestParserWithBrokenNumber()
        {
            using (BibParser parser = new BibParser(new StringReader("@book{,title = 2014")))
            {
                parser.GetAllResults();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UnrecognizableCharacterException))]
        public void TestParserWithUnexpectedCharacter()
        {
            using (BibParser parser = new BibParser(
                            new StringReader("@book{,ti?le = {Hadoop}}")))
            {
                parser.GetAllResults();
            }
        }

        [TestMethod]
        public void TestParserWithBibFile()
        {
			BibParser parser = new BibParser(new StreamReader("TestData/BibParserTest1_In.bib", Encoding.Default));
            var entries = parser.GetAllResults().Item2;

            Assert.AreEqual(4,														entries.Count);
            Assert.AreEqual("nobody",												entries[0].Publisher);
            Assert.AreEqual("Apache hadoop yarn: Yet another resource negotiator",	entries[1].Title);
            Assert.AreEqual("KalavriShang-797",										entries[2].Key);
            parser.Dispose();
        }

        [TestMethod]
        public void TestStaticParseWithBibFile()
        {
            var entries = BibParser.Parse(new StreamReader("TestData/BibParserTest1_In.bib", Encoding.Default));

            Assert.AreEqual(4,														entries.Item2.Count);
            Assert.AreEqual("nobody",												entries.Item2[0].Publisher);
            Assert.AreEqual("Apache hadoop yarn: Yet another resource negotiator",	entries.Item2[1].Title);
            Assert.AreEqual("KalavriShang-797",										entries.Item2[2].Key);
        }

        [TestMethod]
        public void TestParserResult()
        {
            var parser = new BibParser(new StreamReader("TestData/BibParserTest1_In.bib", Encoding.Default));
            var entry = parser.GetAllResults().Item2[0];

            var sr = new StreamReader("TestData/BibParserTest1_Out1.bib", Encoding.Default);
            var expected = sr.ReadToEnd().Replace("\r", "");

            Assert.AreEqual(expected, entry.ToString());

            parser.Dispose();
        }

    } // End class.
} // End namespace.