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
            const string title = "Mapreduce";
            var entry = new BibEntry {["Title"] = title};

            Assert.AreEqual(title, entry["title"]);
            Assert.AreEqual(title, entry["Title"]);
            Assert.AreEqual(title, entry["TitlE"]);
        }

        [TestMethod]
        public void TestProperty()
        {
            const string title = "Mapreduce";
            var entry = new BibEntry {["Title"] = title};

            Assert.AreEqual(title, entry.Title);
        }

        [TestMethod]
        public void TestSetType()
        {
            var entry = new BibEntry {Type = "inbook"};
            Assert.AreEqual("inbook", entry.Type);

            entry.Type = "inBoOK";
            Assert.AreEqual("inBoOK", entry.Type);
        }

        [TestMethod]
        public void TestToString()
        {
            //TODO:
        }

    } // End class.
} // End namespace.