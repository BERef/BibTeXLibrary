using System.Diagnostics;

namespace BibTeXLibrary
{
    internal struct Token
    {
		#region Fields

		public TokenType Type;
        public string Value;

		#endregion

		#region Construction

		public Token(TokenType type, string value = "")
        {
            Type    = type;
            Value   = value;
            Debug.WriteLine(type + "\t" + value);
        }

        #endregion

    } // End class.
} // End namepsace.