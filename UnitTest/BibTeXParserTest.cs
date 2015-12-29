using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BibTeXLibrary;

namespace UnitTest
{
    [TestClass]
    public class BibTeXParserTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var parser = new BibTeXParser(new StringReader("@{keyword, title = {123}, year = \"2012\""));
            parser.GetResult();
        }
    }
}
