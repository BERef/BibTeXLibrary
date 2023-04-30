using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;

namespace BibTeXLibrary
{
	using System.Collections;
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

		#region Construction

        /// <summary>
        /// Default contructor.
        /// </summary>
        public BibEntry()
        {
        }

		#endregion

		#region Private Fields

		/// <summary>
		/// Key
		/// </summary>
		private string _key;

        /// <summary>
        /// Store all tags
        /// </summary>
        private readonly OrderedDictionary _tags = new OrderedDictionary();

        #endregion

        #region Public Properties

		/// <summary>
		/// Address.
		/// </summary>
        public string Address
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// Annote.
		/// </summary>
        public string Annote
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// Author.
		/// </summary>
        public string Author
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// Booktitle.
		/// </summary>
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
        public string Type { get; set; }

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

		#region Private Methods

		/// <summary>
		/// Uses the calling member nameto create a lowercase name to use as an index.
		/// </summary>
		/// <param name="propertyName">Name of the property/calling method.</param>
		private string GetFormattedName([CallerMemberName] string propertyName = null)
		{
			return propertyName.First().ToString().ToLower() + propertyName.Substring(1);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Initialize with a set of (ordered) tags.
		/// </summary>
		public void Initialize(List<string> tags)
		{
			foreach (string tag in tags)
			{
				this[tag] = "";
			}
		}

        /// <summary>
        /// Convert the BibTeX entry to a string.
        /// </summary>
        public override string ToString()
        {
            return ToString(new WriteSettings() { WhiteSpace = WhiteSpace.Space, TabSize = 2, AlignTagValues = false });
        }

		/// <summary>
		/// Convert the BibTeX entry to a string.
		/// </summary>
		/// <param name="writeSettings">The settings for writing the bibliography file.</param>
		public string ToString(WriteSettings writeSettings)
        {
            // Build the entry opening and key.
            var bib = new StringBuilder("@");
            bib.Append(this.Type);
            bib.Append("{");
            bib.Append(Key);
            bib.Append(",");
            bib.Append(writeSettings.NewLine);

			// Write all the tags.
			IDictionaryEnumerator tagEnumerator = _tags.GetEnumerator();
			while (tagEnumerator.MoveNext())
			{
                // Initial line indent and tag key.
                bib.Append(writeSettings.Indent);
                //bib.Append(tag.Key);
                bib.Append(tagEnumerator.Key.ToString());

                // Add the space between the key and equal sign.
                bib.Append(writeSettings.GetInterTagSpacing(tagEnumerator.Key.ToString()));

				// Add the tag value.
				bib.Append("= ");
				bib.Append(tagEnumerator.Value.ToString());

                // End the line.
                bib.Append(writeSettings.NewLine);
            }

            // Closing bracket and end of entry.
            bib.Append("}");
			bib.Append(writeSettings.NewLine);

			return bib.ToString();
        }
        #endregion

        #region Public Tag Value

        /// <summary>
        /// Get value by given tag name (index) or create new tag by index and value.
        /// </summary>
        /// <param name="tagName">Tag name.</param>
        public string this[string tagName]
        {
            get
            {
                tagName = tagName.ToLower();
                return _tags.Contains(tagName) ? ((TagValue)_tags[tagName]).Content : "";
            }
            set
            {
				if (_tags.Contains(tagName))
				{
					((TagValue)_tags[tagName.ToLower()]).Content = value;
				}
				else
				{
					_tags[tagName.ToLower()] = new TagValue(value);
				}
            }
        }

		/// <summary>
		/// Get a TagValue.
		/// </summary>
		/// <param name="tagName">Name of the tag to get.</param>
		public TagValue GetTagValue(string tagName)
		{
			return (TagValue)_tags[tagName.ToLower()];
		}

		/// <summary>
		/// Set a TagValue.
		/// </summary>
		/// <param name="tagName">Name of the tag to get.</param>
		public void SetTagValue(string tagName, TagValue tagValue)
		{
			_tags[tagName.ToLower()] = tagValue;
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
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
		}

		#endregion

	} // End class.
} // End namespace.