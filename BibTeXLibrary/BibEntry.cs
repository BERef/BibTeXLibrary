using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibTeXLibrary
{
    public class BibEntry
    {
        #region Static Field
        #endregion

        #region Private Field
        /// <summary>
        /// Entry's type
        /// </summary>
        private EntryType _type;

        /// <summary>
        /// Store all tags.
        /// </summary>
        private Dictionary<string, string> _tags = new Dictionary<string, string>();
        #endregion

        #region Constructor
        public BibEntry()
        {

        }
        #endregion

        #region Public Property
        public string Address
        {
            get { return this["address"]; }
        }

        public string Annote
        {
            get { return this["annote"]; }
        }

        public string Author
        {
            get { return this["author"]; }
        }

        public string Booktitle
        {
            get { return this["booktitle"]; }
        }

        public string Chapter
        {
            get { return this["chapter"]; }
        }

        public string Crossref
        {
            get { return this["crossref"]; }
        }

        public string Edition
        {
            get { return this["edition"]; }
        }

        public string Editor
        {
            get { return this["editor"]; }
        }

        public string Howpublished
        {
            get { return this["howpublished"]; }
        }

        public string Institution
        {
            get { return this["institution"]; }
        }

        public string Journal
        {
            get { return this["journal"]; }
        }

        public string Key
        {
            get { return this["key"]; }
        }

        public string Mouth
        {
            get { return this["mouth"]; }
        }

        public string Note
        {
            get { return this["note"]; }
        }

        public string Number
        {
            get { return this["number"]; }
        }

        public string Organization
        {
            get { return this["organization"]; }
        }

        public string Pages
        {
            get { return this["pages"]; }
        }

        public string Publisher
        {
            get { return this["publisher"]; }
        }

        public string School
        {
            get { return this["shcool"]; }
        }

        public string Series
        {
            get { return this["series"]; }
        }

        public string Title
        {
            get { return this["title"]; }
        }

        public string Volume
        {
            get { return this["volume"]; }
        }

        public string Year
        {
            get { return this["year"]; }
        }

        public string Type
        {
            get
            {
                return Enum.GetName(typeof(EntryType), _type);
            }
            set
            {
                _type = (EntryType)Enum.Parse(typeof(EntryType), value, true);
            }
        }
        #endregion

        #region Public Method
        #endregion

        #region Public Indexer
        /// <summary>
        /// Get value by given tagname(index) or
        /// create new tag by index and value.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[string index]
        {
            get
            {
                index = index.ToLower();
                return _tags.ContainsKey(index) ? _tags[index] : "";
            }
            set
            {
                _tags[index.ToLower()] = value;
            }
        }
        #endregion

        #region Private Method

        #endregion
    }

    public enum EntryType
    {
        Article,
        Book,
        Booklet,
        Conference,
        InBook,
        InCollection,
        InProceedings,
        Manual,
        Mastersthesis,
        Misc,
        PhDThesis,
        Proceedings,
        TechReport,
        Unpublished
    }
}
