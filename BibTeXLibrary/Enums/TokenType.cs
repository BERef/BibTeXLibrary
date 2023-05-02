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
	public enum TokenType
	{
		/// <summary>Comma.</summary>
		[Description("Comma")]
		Comma,

		/// <summary>Comment.</summary>
		[Description("Comment")]
		Comment,

		/// <summary>Concatenation.</summary>
		[Description("Concatenation")]
		Concatenation,

		/// <summary>Equal.</summary>
		[Description("Equal")]
		Equal,

		/// <summary>End of File.</summary>
		[Description("End of File")]
		EOF,

		/// <summary>LeftBrace.</summary>
		[Description("LeftBrace")]
		LeftBrace,

		/// <summary>Name.</summary>
		[Description("Name")]
		Name,

		/// <summary>RightBrace.</summary>
		[Description("RightBrace")]
		RightBrace,

		/// <summary>Start.</summary>
		[Description("Start")]
		Start,

		/// <summary>String.</summary>
		[Description("String")]
		String,

		/// <summary>The number of types/items in the enumeration.</summary>
		[Description("Length")]
		Length

	} // End enum.
} // End namespace.