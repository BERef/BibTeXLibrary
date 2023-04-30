using System;
using System.Text;

namespace BibTeXLibrary
{
    [Serializable]
    public sealed class UnexpectedTokenException : ParseErrorException
    {
        #region Public Properties

        /// <summary>
        /// Error message.
        /// </summary>
        public override string Message { get; }

        #endregion

        #region Constructor

        public UnexpectedTokenException(int lineNumber, int colNumber, TokenType unexpected, params TokenType[] expected)
            : base(lineNumber, colNumber)
        {
            var errorMsg = new StringBuilder($"An unexpected token was found.\nToken: '{unexpected}'.\nAt line {lineNumber}, column {colNumber}.");

			// Add a list of acceptable tokens.
            errorMsg.Append("\nExpected: ");
            foreach (var item in expected)
            {
                errorMsg.Append($"{item}, ");
            }
			// Remove last comma and space.
            errorMsg.Remove(errorMsg.Length - 2, 2);

            Message = errorMsg.ToString();
        }

        #endregion

    } // End class.
} // End namespace.
