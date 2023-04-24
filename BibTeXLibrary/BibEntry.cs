using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace BibTeXLibrary
{
	using System.Linq;
    using System.Runtime.CompilerServices;

    public class BibEntry : INotifyPropertyChanged
	{
		#region Events
		/// <summary>
		/// Property changed event.  Required by INotifyPropertyChanged.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Private Field
		/// <summary>
		/// Key
		/// </summary>
		private string _key;

		/// <summary>
		/// Entry's type
		/// </summary>
		private EntryType _type;

        /// <summary>
        /// Store all tags
        /// </summary>
        private readonly Dictionary<string, string> _tags = new Dictionary<string, string>();
        #endregion

        #region Public Property
        public string Address
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Annote
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Author
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Booktitle
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Chapter
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Crossref
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Edition
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Editor
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Howpublished
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Institution
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Journal
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Mouth
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Note
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Number
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Organization
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Pages
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Publisher
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string School
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Series
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Title
        {
            get => this[GetFormattedName()];
            set
            {
                this[GetFormattedName()] = value;
                NotifyPropertyChanged("Title");
            }
		}

        public string Volume
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Year
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Month
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        public string Abstract
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        /// <summary>
        /// Entry's type
        /// </summary>
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

        /// <summary>
        /// Entry's key
        /// </summary>
		public string Key
		{
			get
			{
				return _key;
			}
			set
			{
				_key = value;
				NotifyPropertyChanged("Key");
			}
		}
		#endregion

		#region Public Method

		private string GetFormattedName([CallerMemberName] string propertyName = null)
        {
            return propertyName.First().ToString().ToLower() + propertyName.Substring(1);
        }

        /// <summary>
        /// To BibTeX entry
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var bib = new StringBuilder("@");
            bib.Append(Type);
            bib.Append('{');
            bib.Append(Key);
            bib.Append(",");
            bib.Append(Config.LineFeed);

            foreach (var tag in _tags)
            {
                bib.Append(Config.Retract);
                bib.Append(tag.Key);
                bib.Append(" = {");
                bib.Append(tag.Value);
                bib.Append("},");
                bib.Append(Config.LineFeed);
            }

            bib.Append("}");

            return bib.ToString();
        }
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

		#region Property Changed Event Triggering
		/// <summary>
		/// Notify that a property changed.
		/// 
		/// INotifyPropertyChanged Interface
		/// </summary>
		/// <param name="info">Information.</param>
		private void NotifyPropertyChanged(string info)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}
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
        MastersThesis,
        Misc,
        Patent,
        PhDThesis,
        Proceedings,
        TechReport,
        Unpublished,
        WebHref,
    }
}
