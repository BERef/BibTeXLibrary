namespace BibTeXLibrary
{
    public class UnrecognizableCharacterException : ParseErrorException
    {
        #region Public Properties

        /// <summary>
        /// Error message.
        /// </summary>
        public override string Message { get; }

        #endregion

        #region Constructor

        public UnrecognizableCharacterException(int lineNumber, int columnNumber
			, char unexpected)
            : base(lineNumber, columnNumber)
        {
            Message = $"An unexpected character was found.\nCharacter: '{unexpected}'.\nAt line {lineNumber+1}, column {columnNumber+1}.";
        }

        #endregion

    } // End class.
} // End namespace.