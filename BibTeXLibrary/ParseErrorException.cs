using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibTeXLibrary
{
    public abstract class ParseErrorException : ApplicationException
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

        #region Constructor
        public ParseErrorException(int lineNo, int colNo)
        {
            LineNo = lineNo;
            ColNo  = colNo;
        }
        #endregion
    }
}
