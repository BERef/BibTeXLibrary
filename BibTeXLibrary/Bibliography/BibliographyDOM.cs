﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BibTeXLibrary
{
	/// <summary>
	/// Bibliography Document Object Model.
	/// </summary>
	public class BibliographyDOM
	{
		#region Fields

		private List<string>				_header				= new List<string>();
		private BindingList<BibEntry>		_bibEntries			= new BindingList<BibEntry>();
		private List<StringConstantPart>	_strings			= new List<StringConstantPart>();

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public BibliographyDOM()
		{
		}

		/// <summary>
		/// Clean up.
		/// </summary>
		public void Dispose()
		{
			_header		= null;
			_bibEntries = null;
			_strings	= null;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The file header.
		/// </summary>
		public List<string> Header { get { return _header; } }

		/// <summary>
		/// Get the bibliography entries.
		/// </summary>
		public BindingList<BibEntry> BibiographyEntries { get => _bibEntries; }

		/// <summary>
		/// String constants.
		/// </summary>
		public List<StringConstantPart> StringConstants { get => _strings; }

		#endregion

		#region Methods

		/// <summary>
		/// Add a bibliography entry or a string.
		/// </summary>
		/// <param name="part">BibliographyPart.</param>
		public void AddBibPart(BibliographyPart part)
		{
			if (part.Type.ToLower() == "string")
			{
				_strings.Add((StringConstantPart)part);
			}
			else
			{
				_bibEntries.Add((BibEntry)part);
			}
		}

		/// <summary>
		/// Add a line to the header.
		/// </summary>
		/// <param name="line">Line to add.</param>
		public void AddHeaderLine(string line)
		{
			_header.Add(line);
		}

		#endregion

	} // End class.
} // End namespace.