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
            ParserState curState = ParserState.Begin;
            BibEntry bib = null;

            StringBuilder tagValueBuilder = new StringBuilder();
            string tagName = "";

            foreach (var token in Lexer())
            {
                switch(curState)
                {
                    case ParserState.Begin:
                        if(token.Type == TokenType.Start)
                        {
                            curState = ParserState.InStart;
                            bib = new BibEntry();
                        }
                        break;

                    case ParserState.InStart:
                        if(token.Type == TokenType.Name)
                        {
                            curState = ParserState.InEntry;
                            bib.Type = token.Value;
                        }
                        break;

                    case ParserState.InEntry:
                        if (token.Type == TokenType.LeftBrace)
                        {
                            curState = ParserState.InKey;
                        }
                        break;

                    case ParserState.InKey:
                        if (token.Type == TokenType.Name)
                        {
                            curState = ParserState.OutKey;
                            bib.Key = token.Value;
                        }
                        else if(token.Type == TokenType.Comma)
                        {
                            curState = ParserState.InTagName;
                        }
                        break;

                    case ParserState.OutKey:
                        if (token.Type == TokenType.Comma)
                        {
                            curState = ParserState.InTagName;
                        }
                        break;

                    case ParserState.InTagName:
                        if (token.Type == TokenType.Name)
                        {
                            curState = ParserState.InTagEqual;
                            tagName = token.Value;
                            bib[tagName] = "";
                        }
                        break;

                    case ParserState.InTagEqual:
                        if (token.Type == TokenType.Equal)
                        {
                            curState = ParserState.InTagValue;
                        }
                        break;

                    case ParserState.InTagValue:
                        if (token.Type == TokenType.String)
                        {
                            curState = ParserState.OutTagValue;
                            tagValueBuilder.Append(token.Value);
                        }
                        break;

                    case ParserState.OutTagValue:
                        if (token.Type == TokenType.Concatenation)
                        {
                            curState = ParserState.InTagValue;
                        }
                        else 
                        if(token.Type == TokenType.Comma)
                        {
                            curState = ParserState.InTagName;
                            bib[tagName] = tagValueBuilder.ToString();
                            tagValueBuilder.Clear();
                        }
                        else if(token.Type == TokenType.RightBrace)
                        {
                            curState = ParserState.OutEntry;
                            bib[tagName] = tagValueBuilder.ToString();
                            tagValueBuilder.Clear();
                            // Add to result list
                        }
                        break;

                    case ParserState.OutEntry:
                        if (token.Type == TokenType.Start)
                        {
                            curState = ParserState.InStart;
                        }
                        break;
                }
            }
            if(curState != ParserState.OutEntry)
            {
                //TODO: Need Throw an Exception
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

    enum ParserState
    {
        Begin,
        InStart,
        InEntry,
        InKey,
        OutKey,
        InTagName,
        InTagEqual,
        InTagValue,
        OutTagValue,
        OutEntry
    }
}
