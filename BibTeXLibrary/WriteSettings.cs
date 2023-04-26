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

		private WhiteSpace	_whiteSpace					= WhiteSpace.Tab;
		private int			_spacesPerTab				= 4;
		private bool		_alignAtEquals				= true;
		private int			_alignAtColumn				= 20;

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
		public WhiteSpace WhiteSpace { get => _whiteSpace; set => _whiteSpace = value; }

		/// <summary>
		/// The number of spaces per tab to use.
		/// </summary>
		public int SpacesPerTab { get => _spacesPerTab; set => _spacesPerTab = value; }

		/// <summary>
		/// Specifies if the tag values should be aligned at the equal sign.
		/// </summary>
		public bool AlignAtEquals { get => _alignAtEquals; set => _alignAtEquals = value; }

		/// <summary>
		/// Specifies the number of columns before the equal sign when aligning at the equal sign.
		/// </summary>
		public int AlignAtColumn { get => _alignAtColumn; set => _alignAtColumn = value; }

		#endregion

		#region Methods

		#endregion

		#region XML

		#endregion

	} // End class.
} // End namespace.