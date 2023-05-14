using DigitalProduction.Delegates;
using DigitalProduction.Projects;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace BibTeXLibrary
{
	/// <summary>
	/// Settings to use when writing a bib file.
	/// </summary>
	public class WriteSettings : IModified
	{
		#region Events

		/// <summary>
		/// Occurs when the instance is modfied.
		/// </summary>
		public event NoArgumentsEventHandler OnModifiedChanged;

		#endregion

		#region Fields

		private WhiteSpace			_whiteSpace			= WhiteSpace.Tab;
		private int					_tabSize			= 4;
		private bool				_alignTagValues		= true;
		private int					_alignAtColumn		= 24;
		private int					_alignAtTabStop		= 5;
		private bool				_removeLastComma	= true;
		private string				_newLine			= "\n";
		private char				_tab				= '\t';

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WriteSettings()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Specifies if the project has been modified since last being saved/loaded.
		/// </summary>
		[XmlIgnore()]
		public bool Modified
		{
			get
			{
				return false;
			}

			set
			{
				RaiseOnModifiedChangedEvent();
			}
		}

		/// <summary>
		/// Type of white space character to use.
		/// </summary>
		[XmlAttribute("whitespace")]
		public WhiteSpace WhiteSpace
		{
			get
			{
				return _whiteSpace;
			}

			set
			{
				if (_whiteSpace != value)
				{
					_whiteSpace = value;
					this.Modified = true;
				}
			}
		}

		/// <summary>
		/// The number of spaces per tab to use.
		/// </summary>
		[XmlAttribute("spacespertab")]
		public int TabSize
		{
			get
			{
				return _tabSize;
			}
			set
			{
				if (_tabSize != value)
				{
					_tabSize = value;
					this.Modified = true;
				}
			}
		}

		/// <summary>
		/// Specifies if the tag values should be aligned at the equal sign.
		/// </summary>
		[XmlAttribute("alignatequals")]
		public bool AlignTagValues
		{
			get
			{
				return _alignTagValues;
			}

			set
			{
				if (_alignTagValues != value)
				{
					_alignTagValues = value;
					this.Modified = true;
				}
			}
		}

		/// <summary>
		/// Specifies the column number to align tag values at when using spaces as the white space.
		/// </summary>
		[XmlAttribute("alignatcolumn")]
		public int AlignAtColumn
		{
			get
			{
				return _alignAtColumn;
			}
			set
			{
				if (_alignAtColumn != value)
				{
					_alignAtColumn = value;
					this.Modified = true;
				}
			}
		}

		/// <summary>
		/// Specifies the tab stop number to align tag values at when using spaces as the white space.
		/// </summary>
		[XmlAttribute("alignattabstop")]
		public int AlignAtTabStop
		{
			get
			{
				return _alignAtTabStop;
			}

			set
			{
				if (_alignAtTabStop != value)
				{
					_alignAtTabStop = value;
					this.Modified = true;
				}
			}
		}

		/// <summary>
		/// Remove the comma after the last tag in a BibEntry.
		/// </summary>
		[XmlAttribute("removelastcomma")]
		public bool RemoveLastComma
		{
			get
			{
				return _removeLastComma;
			}

			set
			{
				if (_removeLastComma != value)
				{
					_removeLastComma = value;
					this.Modified = true;
				}
			}
		}

		/// <summary>
		/// New line character or sequence.
		/// </summary>
		[XmlIgnore()]
		public string NewLine
		{
			get
			{
				return _newLine;
			}

			set
			{
				if (_newLine != value)
				{
					_newLine = value;
					this.Modified = true;
				}
			}
		}

		/// <summary>
		/// Tab character or sequence.
		/// </summary>
		[XmlIgnore()]
		public char Tab
		{
			get
			{
				return _tab;
			}

			set
			{
				if (_tab != value)
				{
					_tab = value;
					this.Modified = true;
				}
			}
		}

		/// <summary>
		/// Get the same amount of white space a tab would take up as a string of spaces.
		/// </summary>
		private string TabAsSpaces
		{
			get
			{
				return new string(' ', this.TabSize);
			}
		}
		
		/// <summary>
		/// Get a tab or the same amount of spaces as a tab would use.
		/// </summary>
		[XmlIgnore()]
		public string Indent
		{
			get
			{
				switch (this.WhiteSpace)
				{
					case WhiteSpace.Tab:
						return new string(this.Tab, 1);
					
					case WhiteSpace.Space:
						return this.TabAsSpaces;
					
					default:
						throw new InvalidEnumArgumentException("Invalid \"WhiteSpace\" value.");
				}
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Get the space between the tag "key" and the tag "value".
		/// 
		/// Examples:
		/// % Use a space between the key and value (no alignment).
		/// title = {Title of Work}
		/// author = {John Q. Author}
		/// year = {2000}
		/// 
		/// % Align the key and value.
		/// title    = {Title of Work}
		/// author   = {John Q. Author}
		/// year     = {2000}
		/// 
		/// % Use a space between the key and value (no alignment).
		/// </summary>
		/// <param name="tagKey">The tag key as a string.</param>
		public string GetInterTagSpacing(string tagKey)
		{
			if (this.AlignTagValues)
			{
				// To align the values is much more complicated.  First decide if spaces or tabs are going to be inserted.
				switch (this.WhiteSpace)
				{
					case WhiteSpace.Tab:
					{
						// Subtract the initial line indent and the length of the key from the desired number of tabs.
						int requiredTabs = this.AlignAtTabStop - 1 - (int)System.Math.Ceiling((double)(tagKey.Length / this.TabSize));
						if (requiredTabs < 0)
						{
							throw new ArgumentOutOfRangeException("The key is too long for the space allocated for aligning tag values.");
						}
						//int tabs = (int)System.Math.Ceiling((double)requiredTabs / this.TabSize);
						return new string(this.Tab, requiredTabs);
					}
					case WhiteSpace.Space:
					{
						// Subtract the initial line indent and the length of the key from the desired aligning column.
						int requiredSpaces = this.AlignAtColumn - 1 - tagKey.Length - this.TabSize;
						if (requiredSpaces < 0)
						{
							throw new ArgumentOutOfRangeException("The key is too long for the space allocated for aligning tag values.");
						}
						return new string(' ', requiredSpaces);
					}
					default:
					{
						throw new InvalidEnumArgumentException("Invalid \"WhiteSpace\" value.");
					}
				}
			}
			else
			{
				// If we are not aligning values, just return a space.
				return " ";
			}

		}

		/// <summary>
		/// Access for manually firing event for external sources.
		/// </summary>
		private void RaiseOnModifiedChangedEvent()
		{
			if (OnModifiedChanged != null)
			{
				OnModifiedChanged();
			}
		}

		#endregion

	} // End class.
} // End namespace.