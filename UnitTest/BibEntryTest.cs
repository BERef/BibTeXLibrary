using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BibTeXLibrary;

namespace UnitTest
{
    [TestClass]
    public class BibEntryTest
    {
        [TestMethod]
        public void TestIndexer()
        {
            var title = "Mapreduce";
            var entry = new BibEntry();
            entry["Title"] = title;

            Assert.AreEqual(title, entry["title"]);
            Assert.AreEqual(title, entry["Title"]);
            Assert.AreEqual(title, entry["TitlE"]);
        }

        [TestMethod]
        public void TestProperty()
        {
            var title = "Mapreduce";
            var entry = new BibEntry();
            entry["Title"] = title;

            Assert.AreEqual(title, entry.Title);
        }
    }
}
