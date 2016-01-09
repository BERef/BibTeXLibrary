using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibTeXLibrary
{
    public class BibParser : IDisposable
    {
        #region Const Field

        #endregion

        #region Private Field
        /// <summary>
        /// Input text stream.
        /// </summary>
        private readonly TextReader _inputText;

        /// <summary>
        /// Line No. counter
        /// </summary>
        private int _lineCount = 1;
        #endregion

        #region Constructor
        public BibParser(TextReader inputText)
        {
            _inputText = inputText;
        }
        #endregion

        #region Public Method
        public void GetResult()
        {
            Parser();
        }
        #endregion

        #region Private Method
        private void Parser()
        {
            int curState = 0;
            BibEntry bib = null;

            foreach (var token in Lexer())
            {
                

                switch(curState)
                {
                    case 0:
                        if(token.Type == TokenType.Start)
                        {
                            curState++;
                            bib = new BibEntry();
                        }
                        break;

                    case 1:
                        if(token.Type == TokenType.Name)
                        {
                            curState++;
                            bib.Type = token.Value;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Lexer for BibTeX entry.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Token> Lexer()
        {
            int code;
            char c;
            int braceCount = 0;

            while ((code = Peek()) != -1)
            {
                c = (char)code;

                if (c == '@')
                {
                    yield return new Token(TokenType.Start);
                }
                else if (char.IsLetter(c))
                {
                    StringBuilder value = new StringBuilder();

                    while (true)
                    {
                        c = (char)Read();
                        value.Append(c);

                        if ((code = Peek()) == -1) break;
                        c = (char)code;

                        if (!char.IsLetterOrDigit(c)) break;
                    }
                    yield return new Token(TokenType.Name, value.ToString());
                    goto ContinueExcute;
                }
                else if (char.IsDigit(c))
                {
                    StringBuilder value = new StringBuilder();

                    while (true)
                    {
                        c = (char)Read();
                        value.Append(c);

                        if ((code = Peek()) == -1) break;
                        c = (char)code;

                        if (!char.IsDigit(c)) break;
                    }
                    yield return new Token(TokenType.String, value.ToString());
                    goto ContinueExcute;
                }
                else if (c == '"')
                {
                    StringBuilder value = new StringBuilder();

                    _inputText.Read();
                    while ((code = Peek()) != -1)
                    {
                        if (c != '\\' && code == '"') break;

                        c = (char)Read();
                        value.Append(c);
                        
                    }
                    yield return new Token(TokenType.String, value.ToString());
                }
                else if (c == '{')
                {
                    if (braceCount++ == 0)
                    {
                        yield return new Token(TokenType.LeftBrace);
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
                        if (braceCount > 1)
                        {
                            //TODO: need throw an exception
                        }
                        yield return new Token(TokenType.String, value.ToString());
                        goto ContinueExcute;
                    }
                }
                else if (c == '}')
                {
                    yield return new Token(TokenType.RightBrace);
                }
                else if (c == ',')
                {
                    yield return new Token(TokenType.Comma);
                }
                else if (c == '#')
                {
                    yield return new Token(TokenType.Concatenation);
                }
                else if (c == '=')
                {
                    yield return new Token(TokenType.Equal);
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
                if (_inputText.Peek() != -1)
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

        #region Impement Interface "IDisposable"
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
