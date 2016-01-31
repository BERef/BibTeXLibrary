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
            var e = new UnrecognizableCharacterException(1, 10, '?');
            Assert.AreEqual(1,  e.LineNo);
            Assert.AreEqual(10, e.ColNo);
            Assert.AreEqual("Line 1, Col 10. Unrecognizable character: '?'.", e.Message);
        }
    }
}
