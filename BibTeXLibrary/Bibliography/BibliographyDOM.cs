using DigitalProduction.Strings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

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

		#region Add Methods

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

		#region Organization Methods

		/// <summary>
		/// Sort the bibliography entries.
		/// </summary>
		/// <param name="sortBy">Method to sort the bibliography entries by.</param>
		/// <exception cref="ArgumentException">The SortBy method is not valid.</exception>
		public void SortBibEntries(SortBy sortBy)
		{
			// The copy constructor doesn't work, it points to the _bibEntry list and when that list is cleared, both are cleared (and the enumerators).
			BindingList<BibEntry> copy = new BindingList<BibEntry>();
			foreach (BibEntry entry in _bibEntries)
			{
				copy.Add(entry);
			}

			IOrderedEnumerable<BibEntry> sorted;
			switch (sortBy)
			{
				case SortBy.FirstAuthorLastName:
					sorted = copy.OrderBy(entry => entry.GetFirstAuthorsName(NameFormat.Last, StringCase.LowerCase));
					break;

				case SortBy.Key:
					sorted = copy.OrderBy(entry => entry.Key);
					break;

				default:
					throw new ArgumentException("The specified method of sorting is not valid.");
			}

			_bibEntries.Clear();
			foreach (BibEntry entry in sorted)
			{
				_bibEntries.Add(entry);
			}
		}

		#endregion

	} // End class.
} // End namespace.