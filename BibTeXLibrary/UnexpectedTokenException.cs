using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibTeXLibrary
{
    [Serializable]
    public sealed class UnexpectedTokenException : ApplicationException
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
        public UnexpectedTokenException(int lineNo, int colNo, TokenType unexpected, params TokenType[] expected)
        {
            var errorMsg = new StringBuilder($"Unexpected token: {unexpected.ToString()}. ");
            errorMsg.Append("Expected: ");
            foreach (var item in expected)
            {
                errorMsg.Append($"{item.ToString()}, ");
            }
            errorMsg.Remove(errorMsg.Length - 2, 2);
            
            _message = errorMsg.ToString();
            LineNo = lineNo;
            ColNo  = colNo;
        }
        #endregion
    }
}
