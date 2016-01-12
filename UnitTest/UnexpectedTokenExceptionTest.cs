using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BibTeXLibrary;

namespace UnitTest
{
    [TestClass]
    public class UnexpectedTokenExceptionTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var e = new UnexpectedTokenException(1, 10, TokenType.EOF, TokenType.Comma, TokenType.RightBrace);
            Assert.AreEqual(1, e.LineNo);
            Assert.AreEqual(10, e.ColNo);
            Assert.AreEqual("Unexpected token: EOF. Expected: Comma, RightBrace", e.Message);
        }
    }
}
