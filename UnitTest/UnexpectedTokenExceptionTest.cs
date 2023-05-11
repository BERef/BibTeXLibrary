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
            var exception = new UnexpectedTokenException(1, 10, TokenType.EOF, TokenType.Comma, TokenType.RightBrace);
            Assert.AreEqual(1,  exception.LineNumber);
            Assert.AreEqual(10, exception.ColumnNumber);
            Assert.AreEqual("An unexpected token was found.\nToken: 'EOF'.\nAt line 1, column 10.\nExpected: Comma, RightBrace", exception.Message);
        }

	} // End class.
} // End namespace.
