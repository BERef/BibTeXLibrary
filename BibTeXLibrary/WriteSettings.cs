using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BibTeXLibrary
{
	/// <summary>
	/// Settings to use when writing a bib file.
	/// </summary>
	public class WriteSettings
	{
		#region Members

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
		/// Type of white space character to use.
		/// </summary>
		[XmlAttribute("whitespace")]
		public WhiteSpace WhiteSpace { get; set; } = WhiteSpace.Tab;

		/// <summary>
		/// The number of spaces per tab to use.
		/// </summary>
		[XmlAttribute("spacespertab")]
		public int SpacesPerTab { get; set; } = 4;

		/// <summary>
		/// Specifies if the tag values should be aligned at the equal sign.
		/// </summary>
		[XmlAttribute("alignatequals")]
		public bool AlignAtEquals { get; set; } = true;

		/// <summary>
		/// Specifies the number of columns before the equal sign when aligning at the equal sign.
		/// </summary>
		[XmlAttribute("alignatcolumn")]
		public int AlignAtColumn { get; set; } = 20;

		/// <summary>
		/// New line character or sequence.
		/// </summary>
		[XmlIgnore()]
		public string NewLine { get; set; } = "\n";

		[XmlIgnore()]
		public string LineIndent
		{
			get
			{
				switch (this.WhiteSpace)
				{
					case WhiteSpace.Tab:
					{
						return "\t";
					}
					case WhiteSpace.Space:
					{
						return new string(' ', this.SpacesPerTab);
					}
					default:
					{
						throw new InvalidEnumArgumentException("Invalid \"WhiteSpace\" value.");
					}
				}
			}
		}

		#endregion

		#region Methods

		#endregion

		#region XML

		#endregion

	} // End class.
} // End namespace.