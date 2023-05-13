using System.ComponentModel;

namespace BibTeXLibrary
{
	/// <summary>
	/// The state of the parser.
	/// 
	/// The "Description" attribute can be accessed using Reflection to get a string representing the enumeration type.
	/// 
	/// The "Length" enumeration can be used in loops as a convenient way of terminating a loop that does not have to be changed if
	/// the number of items in the enumeration changes.  The "Length" enumeration must be the last item.
	/// for (int i = 0; i &lt; (int)EnumType.Length; i++) {...}
	/// </summary>
	public enum ParserState
	{
		/// <summary>Begin.</summary>
		[Description("Begin")]
		Begin,

		/// <summary>In the header.</summary>
		[Description("In Header")]
		InHeader,

		/// <summary>At the start of a bibliography entry where the type is written..</summary>
		[Description("Bibliography Entry Type")]
		InStart,

		/// <summary>Immediatly after the start of an entry and before the cite key (the opening brace).</summary>
		[Description("In Entry")]
		InEntry,

		/// <summary>In the cite key.</summary>
		[Description("In Cite Key")]
		InKey,

		/// <summary>After the cite key.</summary>
		[Description("Out of Cite Key")]
		OutKey,

		/// <summary>Immediatly after the start of a string constant and before the string definition (the opening brace).</summary>
		[Description("In String Definition")]
		InStringEntry,

		/// <summary>In the name of a tag.</summary>
		[Description("In Tag Name")]
		InTagName,

		/// <summary>At the equal sign betwen a tag name and value.</summary>
		[Description("In Tag Equal")]
		InTagEqual,

		/// <summary>In the value of a tag.</summary>
		[Description("In Tag Value")]
		InTagValue,

		/// <summary>Finish reading the tag value.</summary>
		[Description("Out of Tag Value")]
		OutTagValue,

		/// <summary>Read a comment.</summary>
		[Description("In comment")]
		InComment,

		/// <summary>At the end of an entry (the closing brace).</summary>
		[Description("Out of Entry")]
		OutEntry,

		/// <summary>The number of types/items in the enumeration.</summary>
		[Description("Length")]
		Length

	} // End enum.
} // End namespace.