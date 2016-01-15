using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibTeXLibrary
{
    class UnexpectedCharacterException : ApplicationException
    {
        #region Private Field
        /// <summary>
        /// Customed error message.
        /// </summary>
        private readonly string _message;
        #endregion

        #region Public Field
        /// <summary>
        /// Line No.
        /// </summary>
        public readonly int LineNo;

        /// <summary>
        /// Col No.
        /// </summary>
        public readonly int ColNo;
        #endregion

        #region Public Property
        /// <summary>
        /// Error message.
        /// </summary>
        public override string Message
        {
            get { return _message; }
        }
        #endregion

        #region Constructor
        public UnexpectedCharacterException(int lineNo, int colNo, char unexpected)
        {
            var errorMsg = new StringBuilder(
                $"Line {lineNo}, Col {colNo}. Unexpected character: {unexpected.ToString()}. ");

            _message = errorMsg.ToString();
            LineNo = lineNo;
            ColNo = colNo;
        }
        #endregion
    }
}
