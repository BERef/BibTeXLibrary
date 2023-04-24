using System.ComponentModel;

namespace BibTeXLibrary
{
	/// <summary>
	/// Add summary here.
	/// 
	/// The "Description" attribute can be accessed using Reflection to get a string representing the enumeration type.
	/// 
	/// The "Length" enumeration can be used in loops as a convenient way of terminating a loop that does not have to be changed if
	/// the number of items in the enumeration changes.  The "Length" enumeration must be the last item.
	/// for (int i = 0; i &lt; (int)EnumType.Length; i++) {...}
	/// </summary>
	public enum EntryType
	{
		/// <summary>Article</summary>
		[Description("Article")]
		Article,
		Book,
		Booklet,
		Conference,
		InBook,
		InCollection,
		InProceedings,
		Manual,
		MastersThesis,
		Misc,
		Patent,
		PhDThesis,
		Proceedings,
		TechReport,
		Unpublished,
		WebHref

	} // End enum.
} // End namespace.