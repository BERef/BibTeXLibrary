using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibTeXLibrary
{
    public class UnexpectedCharacterException : ParserErrorException
    {
        #region Private Field
        /// <summary>
        /// Customed error message.
        /// </summary>
        private readonly string _message;
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
            : base(lineNo, colNo)
        {
            _message = $"Line {lineNo}, Col {colNo}. Unexpected character: {unexpected.ToString()}.";
        }
        #endregion
    }
}
