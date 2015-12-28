using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibTeXLibrary
{
    public class BibTeXParser : IDisposable
    {
        #region Private field
        /// <summary>
        /// Input text stream.
        /// </summary>
        private readonly TextReader _inputText;
        #endregion

        #region Constructor
        public BibTeXParser(TextReader inputText)
        {
            _inputText = inputText;
        }
        #endregion

        #region Impement interface "IDisposable"
        /// <summary>
        /// Dispose stream resource.
        /// </summary>
        public void Dispose()
        {
            _inputText.Dispose();
        }
        #endregion
    }
}
