using Microsoft.VisualStudio.TestTools.UnitTesting;

using BibTeXLibrary;

namespace UnitTest
{
    [TestClass]
    public class UnrecognizableCharacterExceptionTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var exception = new UnrecognizableCharacterException(1, 10, '?');
            Assert.AreEqual(1,  exception.LineNumber);
            Assert.AreEqual(10, exception.ColumnNumber);
            Assert.AreEqual("An unexpected character was found.\nCharacter: '?'.\nAt line 2, column 11.", exception.Message);
		}

	}// End class.
} // End namespace.
