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

        /// <summary>
        /// Line No counter
        /// </summary>
        private int _lineCount = 1;
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
            int braceCount = 0;

            while ((code = Peek()) != -1)
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
                        c = (char)Read();
                        value.Append(c);

                        if ((code = Peek()) == -1) break;
                        c = (char)code;

                        if (!char.IsLetterOrDigit(c)) break;
                    }
                    new Token(TokenType.String, value.ToString());
                    goto ContinueExcute;
                }
                else if (c == '"')
                {
                    StringBuilder value = new StringBuilder();

                    _inputText.Read();
                    while((code = Peek()) != -1)
                    {
                        c = (char)Read();
                        if (c == '"') break;

                        value.Append(c);
                    }
                    new Token(TokenType.String, value.ToString());
                    goto ContinueExcute;
                }
                else if (c == '{')
                {
                    if (braceCount++ == 0)
                    {
                        new Token(TokenType.LeftBrace);
                    }
                    else
                    {
                        StringBuilder value = new StringBuilder();

                        Read();
                        while (braceCount > 1 && (code = Peek()) != -1)
                        {
                            c = (char)Read();
                            if (c == '{') braceCount++;
                            else if (c == '}') braceCount--;
                            if (braceCount > 1) value.Append(c);
                        }
                        if(braceCount > 1)
                        {
                            //TODO: need throw an exception
                        }
                        new Token(TokenType.String, value.ToString());
                        goto ContinueExcute;
                    }
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
                else if (c == '=')
                {
                    new Token(TokenType.Equal);
                }
                else if (c == '\n')
                {
                    _lineCount++;
                }
                else if (!char.IsWhiteSpace(c))
                {
                    //TODO: need throw an exception
                }

                // Move to next char if possible
                if(_inputText.Peek() != -1)
                    _inputText.Read();

            // Don't move
            ContinueExcute: continue;
            }
        }

        /// <summary>
        /// Peek next char but not move forward.
        /// </summary>
        /// <returns></returns>
        private int Peek()
        {
            return _inputText.Peek();
        }

        /// <summary>
        /// Read next char and move forward.
        /// </summary>
        /// <returns></returns>
        private int Read()
        {
            return _inputText.Read();
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
