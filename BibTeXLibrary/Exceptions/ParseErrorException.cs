using System;

namespace BibTeXLibrary
{
    public abstract class ParseErrorException : ApplicationException
    {
        #region Public Fields

        /// <summary>
        /// Line No.
        /// </summary>
        public readonly int LineNumber;

        /// <summary>
        /// Col No.
        /// </summary>
        public readonly int ColumnNumber;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor with parsing error location.
		/// </summary>
		/// <param name="lineNumber">The line number of the error.</param>
		/// <param name="columnNumber">The column number of the error.</param>
		protected ParseErrorException(int lineNumber, int columnNumber)
        {
            LineNumber		= lineNumber;
            ColumnNumber	= columnNumber;
        }

        #endregion

    } // End class.
} // End namespace.
