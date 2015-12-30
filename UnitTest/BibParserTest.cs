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
        public void TestMethod1()
        {
            var parser = new BibParser(
                new StringReader("@article{keyword, title = {\"0\"{123}456{789}}, year = \"2012\"}"));
            parser.GetResult();
        }
    }
}
