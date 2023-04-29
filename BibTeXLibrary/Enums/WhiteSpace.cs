using System.ComponentModel;

namespace BibTeXLibrary
{
	/// <summary>
	/// The type of character to fill white space with.
	/// 
	/// The "Description" attribute can be accessed using Reflection to get a string representing the enumeration type.
	/// 
	/// The "Length" enumeration can be used in loops as a convenient way of terminating a loop that does not have to be changed if
	/// the number of items in the enumeration changes.  The "Length" enumeration must be the last item.
	/// for (int i = 0; i &lt; (int)EnumType.Length; i++) {...}
	/// </summary>
	public enum WhiteSpace
	{
		/// <summary>Use spaces for white space.</summary>
		[Description("Space")]
		Space,

		/// <summary>Use tabs for white space.</summary>
		[Description("Tab")]
		Tab,

		/// <summary>The number of types/items in the enumeration.</summary>
		[Description("Length")]
		Length

	} // End enum.
} // End namespace.