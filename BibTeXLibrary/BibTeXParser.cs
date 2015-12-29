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
        #region Const field

        #endregion

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

        #region Public method
        public void GetResult()
        {
            Parser();
        }
        #endregion

        #region Private method
        private void Parser()
        {
            int code;
            char c;

            while ((code = _inputText.Peek()) != -1)
            {
                c = (char)code;

                if (c == '@')
                {
                    new Token(TokenType.Start);
                }
                else if (char.IsLetterOrDigit(c))
                {
                    StringBuilder value = new StringBuilder();
                    
                    while(true)
                    {
                        c = (char)_inputText.Read();
                        value.Append(c);

                        if ((code = _inputText.Peek()) == -1) break;
                        c = (char)code;

                        if (!char.IsLetterOrDigit(c)) break;
                    }
                    new Token(TokenType.String, value.ToString());
                }
                else if (c == '"')
                {
                    _inputText.Read();
                    
                    StringBuilder value = new StringBuilder();
                    
                    while((code = _inputText.Peek()) != -1)
                    {
                        c = (char)_inputText.Read();
                        if (c == '"') break;

                        value.Append(c);
                    }
                    new Token(TokenType.String, value.ToString());
                }
                else if (c == '{')
                {
                    new Token(TokenType.LeftBrace);
                }
                else if (c == '}')
                {
                    new Token(TokenType.RightBrace);
                }
                else if (c == ',')
                {
                    new Token(TokenType.Comma);
                }
                else if (c == '#')
                {
                    new Token(TokenType.Concatenation);
                }
                else if (!char.IsWhiteSpace(c))
                {
                    //TODO: need throw an exception
                }
                // Move to next char
                _inputText.Read();
            }
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
