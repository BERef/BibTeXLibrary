using DigitalProduction.Strings;
using System.Collections.Generic;

namespace BibTeXLibrary
{
	/// <summary>
	/// Compare BibEntrys by the last name of the first author.
	/// </summary>
	public class CompareByFirstAuthorLastName : IComparer<BibEntry>
	{
		#region Fields

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public CompareByFirstAuthorLastName()
		{
		}

		#endregion

		#region Properties

		#endregion

		#region Methods

		/// <summary>
		/// Compare two BibEntrys.
		/// </summary>
		/// <param name="entry1">The first BibEntry.</param>
		/// <param name="entry2">The second BibEntry.</param>
		public int Compare(BibEntry entry1, BibEntry entry2)
		{
			if (entry1 == null && entry2 == null) return 0;
			if (entry1 == null) return -1;
			if (entry2 == null) return 1;
			return GetComparisonName(entry1).CompareTo(GetComparisonName(entry2));
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <param name="entry"></param>
		private string GetComparisonName(BibEntry entry)
		{
			return entry.GetFirstAuthorsName(NameFormat.Last, StringCase.LowerCase);
		}

		#endregion

	} // End class.
} // End namespace.