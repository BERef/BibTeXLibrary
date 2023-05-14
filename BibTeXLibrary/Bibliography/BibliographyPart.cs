using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BibTeXLibrary
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class BibliographyPart : INotifyPropertyChanged
	{
		#region Events

		/// <summary>
		/// Property changed event.  Required by INotifyPropertyChanged.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Fields

		/// <summary>Bibliography entry type, e.g. "book", "thesis", "string".</summary>
		protected string                            _type;

		/// <summary>Cite key.</summary>
		protected string                            _key;

		/// <summary>Store all tags.</summary>
		protected readonly OrderedDictionary        _tags						= new OrderedDictionary();

		/// <summary>Specifies if the tags are case sensitive.</summary>
		private bool                                _caseSensitivetags;

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public BibliographyPart(bool caseSensitivetags)
		{
			_caseSensitivetags = caseSensitivetags;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Entry's type.
		/// </summary>
		public string Type { get => _type; set => _type = value; }

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

		#region Public Methods

		public string FindTagValue(string value, bool caseSensitive = false)
		{
			string result = "";
			string matchValue = caseSensitive ? value : value.ToLower();

			IDictionaryEnumerator tagEnumerator = _tags.GetEnumerator();
			while (tagEnumerator.MoveNext())
			{
				string tagValue = tagEnumerator.Value.ToString();
				if (!caseSensitive)
				{
					tagValue = tagValue.ToLower();
				}

				if (tagValue == matchValue)
				{
					result = tagEnumerator.Key.ToString();
					break;
				}
			}

			return result;
		}

		#endregion

		#region Public Tag Value Methods

		/// <summary>
		/// Get value by given tag name (index) or create new tag by index and value.
		/// </summary>
		/// <param name="tagName">Tag name.</param>
		public string this[string tagName]
		{
			get
			{
				if (!_caseSensitivetags)
				{
					tagName = tagName.ToLower();
				}
				return _tags.Contains(tagName) ? ((TagValue)_tags[tagName]).Content : "";
			}
			set
			{
				if (!_caseSensitivetags)
				{
					tagName = tagName.ToLower();
				}

				if (_tags.Contains(tagName))
				{
					((TagValue)_tags[tagName]).Content = value;
				}
				else
				{
					_tags[tagName] = new TagValue(value);
				}
			}
		}

		/// <summary>
		/// Get a TagValue.
		/// </summary>
		/// <param name="tagName">Name of the tag to get.</param>
		public TagValue GetTagValue(string tagName)
		{
			if (!_caseSensitivetags)
			{
				tagName = tagName.ToLower();
			}
			return (TagValue)_tags[tagName];
		}

		/// <summary>
		/// Set a TagValue.
		/// </summary>
		/// <param name="tagName">Name of the tag to get.</param>
		public void SetTagValue(string tagName, string tagValue)
		{
			if (_caseSensitivetags)
			{
				tagName = tagName.ToLower();
			}
			TagValue tagValueObject		= GetTagValue(tagName);
			tagValueObject.Content		= tagValue;
			_tags[tagName]				= tagValueObject;
		}

		/// <summary>
		/// Set a TagValue.
		/// </summary>
		/// <param name="tagName">Name of the tag to get.</param>
		public void SetTagValue(string tagName, TagValue tagValue)
		{
			if (!_caseSensitivetags)
			{
				tagName = tagName.ToLower();
			}
			_tags[tagName] = tagValue;
		}

		#endregion

		#region Public String Writing Methods

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
		public abstract string ToString(WriteSettings writeSettings);

		#endregion

		#region Property Changed Event Triggering

		/// <summary>
		/// Notify that a property changed.
		/// 
		/// INotifyPropertyChanged Interface
		/// </summary>
		/// <param name="info">Information.</param>
		protected void NotifyPropertyChanged(string info)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
		}

		#endregion

	} // End class.
} // End namespace.