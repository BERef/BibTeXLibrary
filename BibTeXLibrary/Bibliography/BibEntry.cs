using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace BibTeXLibrary
{

	/// <summary>
	/// A bibliography entry.
	/// </summary>
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

		#region Public Static Methods

		/// <summary>
		/// Create a new BibEntry template.  The template is an initialized, but blank BibEntry.
		/// </summary>
		/// <param name="bibEntryInitialization">BibEntryInitialization.</param>
		/// <param name="type">The "type" of the bibliography entry.  The type must have an initialization template.</param>
		public static BibEntry NewBibEntryTemplate(BibEntryInitialization bibEntryInitialization, string type)
		{
			BibEntry bibEntry = new BibEntry() { Type = type };
			bibEntry.Initialize(bibEntryInitialization.GetDefaultTags(type));

			return bibEntry;
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
		/// The address entry or an empty string if the address was not specified.
		/// </summary>
		public string Address
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The annote entry or an empty string if the annote was not specified.
		/// </summary>
		public string Annote
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The author entry or an empty string if the author was not specified.
		/// </summary>
		public string Author
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The booktitle entry or an empty string if the booktitle was not specified.
		/// </summary>
		public string BookTitle
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The chapter entry or an empty string if the chapter was not specified.
		/// </summary>
		public string Chapter
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The crossref entry or an empty string if the crossref was not specified.
		/// </summary>
		public string CrossRef
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The edition entry or an empty string if the edition was not specified.
		/// </summary>
		public string Edition
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The editor entry or an empty string if the editor was not specified.
		/// </summary>
		public string Editor
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The howpublished entry or an empty string if the howpublished was not specified.
		/// </summary>
		public string HowPublished
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The institution entry or an empty string if the institution was not specified.
		/// </summary>
		public string Institution
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The journal entry or an empty string if the journal was not specified.
		/// </summary>
		public string Journal
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The note entry or an empty string if the note was not specified.
		/// </summary>
		public string Note
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The number entry or an empty string if the number was not specified.
		/// </summary>
		public string Number
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The organization entry or an empty string if the organization was not specified.
		/// </summary>
		public string Organization
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The pages entry or an empty string if the pages was not specified.
		/// </summary>
		public string Pages
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The publisher entry or an empty string if the publisher was not specified.
		/// </summary>
		public string Publisher
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The school entry or an empty string if the school was not specified.
		/// </summary>
		public string School
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The series entry or an empty string if the series was not specified.
		/// </summary>
		public string Series
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The title entry or an empty string if the title was not specified.
		/// </summary>
		public string Title
        {
            get => this[GetFormattedName()];
            set
            {
                this[GetFormattedName()] = value;
                NotifyPropertyChanged("Title");
            }
		}

		/// <summary>
		/// The volume entry or an empty string if the volume was not specified.
		/// </summary>
        public string Volume
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The year entry or an empty string if the year was not specified.
		/// </summary>
		public string Year
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The month entry or an empty string if the month was not specified.
		/// </summary>
		public string Month
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

		/// <summary>
		/// The abstract entry or an empty string if the abstract was not specified.
		/// </summary>
		public string Abstract
        {
            get => this[GetFormattedName()];
            set => this[GetFormattedName()] = value;
        }

        /// <summary>
        /// Entry's type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Entry's key.
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

		/// <summary>
		/// Get the names of the tags.
		/// </summary>
		public List<string> TagNames { get => (from string item in _tags.Keys select item).ToList(); }

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
				bib.Append(",");

				// End the line.
				bib.Append(writeSettings.NewLine);
            }
			// Option to remove comma after last tag.
			if (writeSettings.RemoveLastComma)
			{
				// Remove comma after the last tag.  To do that, we need to remove the new line character and the
				// comma and then replace it with a new line character.
				bib.Remove(bib.Length - 1 - writeSettings.NewLine.Length, 1 + writeSettings.NewLine.Length);
				bib.Append(writeSettings.NewLine);
			}

            // Closing bracket and end of entry.
            bib.Append("}");
			bib.Append(writeSettings.NewLine);

			return bib.ToString();
        }

		#endregion

		#region Public Tag Keys

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
		/// Change the Key of a tag.
		/// </summary>
		/// <param name="tagKey">Tag Key to change.</param>
		/// <param name="newTagKey">New tag Key.</param>
		/// <exception cref="ArgumentException">Thrown if the new tag Key already exists.</exception>
		public void RenameTagKey(string tagKey, string newTagKey)
		{
			List<string> tagNames = this.TagNames;

			// It should have already been checked that the key is contained before getting here.
			System.Diagnostics.Debug.Assert(tagNames.Contains(tagKey));

			TagValue value = GetTagValue(tagKey);

			_tags.Remove(tagKey);

			if (tagNames.Contains(newTagKey))
			{
				// The new tag key can exist, but it must be empty.  Don't overwrite existing content.
				if (GetTagValue(newTagKey).Content != "")
				{
					throw new ArgumentException("The tag key \"" + newTagKey + "\" already exists.");
				}

				_tags[newTagKey] = value;
			}
			else
			{
				_tags.Add(newTagKey, value);
			}
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