using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibTeXLibrary
{
    public class UnrecognizableCharacterException : ParseErrorException
    {
        #region Public Property
        /// <summary>
        /// Error message.
        /// </summary>
        public override string Message { get; }

        #endregion

        #region Constructor
        public UnrecognizableCharacterException(int lineNo, int colNo, char unexpected)
            : base(lineNo, colNo)
        {
            Message = $"Line {lineNo}, Col {colNo}. Unrecognizable character: '{unexpected}'.";
        }
        #endregion
    }
}
