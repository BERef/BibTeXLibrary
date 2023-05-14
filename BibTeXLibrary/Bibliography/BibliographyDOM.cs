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
		public BindingList<BibEntry> BibliographyEntries { get => _bibEntries; }

		/// <summary>
		/// The number of bibliography entries.
		/// </summary>
		public int NumberOfBibliographyEntries { get => _bibEntries.Count; }

		/// <summary>
		/// String constants.
		/// </summary>
		public List<StringConstantPart> StringConstants { get => _strings; }

		/// <summary>
		/// The number of string constants.
		/// </summary>
		public int NumberOfStringConstants { get => _strings.Count; }

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

		public int FindInsertIndex(BibEntry entry, SortBy sortBy)
		{
			return BinarySearch<BibEntry>(_bibEntries, entry, new CompareByFirstAuthorLastName(), false);
		}

		/// <summary>
		/// A binarySearch.
		/// </summary>
		/// <typeparam name="T">Template type.</typeparam>
		/// <param name="list">The list of items to search.</param>
		/// <param name="item">The item to search for.</param>
		/// <param name="comparer">Comparison function.</param>
		/// <param name="itemMustExist">If the item must exist, an error is thrown if the item is not found.  Otherwise, the position where the item would be found is returned.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public int BinarySearch<T>(BindingList<T> list, T item, IComparer<T> comparer, bool itemMustExist = true)
		{
			int min = 0;
			int max = list.Count - 1;

			while (min <= max)
			{
				int mid     = (min + max) / 2;
				int result  = comparer.Compare(item, list[mid]);

				if (result == 0)
				{
					return ++mid;
				}
				else if (result < 0)
				{
					max = mid - 1;
				}
				else
				{
					min = mid + 1;
				}
			}

			if (itemMustExist)
			{
				throw new Exception("The item");
			}

			return min;
		}

		private string GetComparisonName(BibEntry entry)
		{
			return entry.GetFirstAuthorsName(NameFormat.Last, StringCase.LowerCase);
		}

		#endregion

	} // End class.

	public class CompareByFirstAuthorLastName : IComparer<BibEntry>
	{
		public int Compare(BibEntry entry1, BibEntry entry2)
		{
			if (entry1 == null && entry2 == null) return 0;
			if (entry1 == null) return -1;
			if (entry2 == null) return 1;
			return GetComparisonName(entry1).CompareTo(GetComparisonName(entry2));
		}

		private string GetComparisonName(BibEntry entry)
		{
			return entry.GetFirstAuthorsName(NameFormat.Last, StringCase.LowerCase);
		}
	}	

} // End namespace.