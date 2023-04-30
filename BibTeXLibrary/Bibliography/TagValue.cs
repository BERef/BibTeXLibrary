namespace BibTeXLibrary
{
	/// <summary>
	/// The tag value for a BibTeX library.
	/// </summary>
	public class TagValue
	{
		#region Fields

		private string				_content;
		private bool				_isString		= true;

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TagValue()
		{
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="content">The tag content.</param>
		public TagValue(string content)
		{
			_content = content;
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="content">The tag content.</param>
		/// <param name="isString">Specifies if the tag value string.  If false, the value is a "Name" (named BibTeX @string).</param>
		public TagValue(string content, bool isString)
		{
			_content	= content;
			_isString	= isString;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The content of the tag value.
		/// </summary>
		public string Content { get => _content; set => _content = value; }

		/// <summary>
		/// Specifies is the value is a common entry (text) or a BibTeX string.
		/// </summary>
		public bool IsString { get => _isString; set => _isString = value; }

		#endregion

		#region Methods

		/// <summary>
		/// Get a string representation.
		/// </summary>
		public override string ToString()
		{
			return _isString ? "{"+_content+"}" : _content;
		}

		#endregion

	} // End class.
} // End namespace.