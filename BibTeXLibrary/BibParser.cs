using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibTeXLibrary
{
    using Next = Dictionary<TokenType, ParserState>;
    using StateMap = Dictionary<ParserState, Dictionary<TokenType, ParserState>>;

    public class BibParser : IDisposable
    {
        #region Const Field
        /// <summary>
        /// State tranfer map
        /// </summary>
        private static readonly StateMap _stateMap = new StateMap
        {
            {ParserState.Begin, new Next {
                { TokenType.Start, ParserState.InStart } } },
            {ParserState.InStart, new Next {
                { TokenType.Name, ParserState.InEntry } } },
            {ParserState.InEntry, new Next {
                { TokenType.LeftBrace, ParserState.InKey } } },
            {ParserState.InKey, new Next {
                { TokenType.Name, ParserState.OutKey },
                { TokenType.Comma, ParserState.InTagName } } },
            {ParserState.OutKey, new Next {
                { TokenType.Comma, ParserState.InTagName } } },
            {ParserState.InTagName, new Next {
                { TokenType.Name, ParserState.InTagEqual } } },
            {ParserState.InTagEqual, new Next {
                { TokenType.Equal, ParserState.InTagValue } } },
            {ParserState.InTagValue, new Next {
                { TokenType.String, ParserState.OutTagValue },
                /*{ TokenType.Name, ParserState.OutTagValue }*/ } },
            {ParserState.OutTagValue, new Next {
                { TokenType.Concatenation, ParserState.InTagValue },
                { TokenType.Comma, ParserState.InTagName },
                { TokenType.RightBrace, ParserState.OutEntry } } },
            {ParserState.OutEntry, new Next {
                { TokenType.Start, ParserState.InStart } } },
        }; 
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
            var curState = ParserState.Begin;
            var nextState = ParserState.Begin;
            BibEntry bib = null;

            StringBuilder tagValueBuilder = new StringBuilder();
            string tagName = "";

            foreach (var token in Lexer())
            {
                if(_stateMap[curState].ContainsKey(token.Type))
                {
                    nextState = _stateMap[curState][token.Type];
                }
                else
                {
                    //TODO: need to thrown an exception
                }
                switch(curState)
                {
                    case ParserState.Begin:
                        if(TryMatch(token, TokenType.Start))
                        {
                            bib = new BibEntry();
                        }
                        break;
                        
                    case ParserState.InKey:
                        if (TryMatch(token, TokenType.Name))
                        {
                            bib.Key = token.Value;
                        }
                        break;

                    case ParserState.InTagName:
                        if (TryMatch(token, TokenType.Name))
                        {
                            tagName = token.Value;
                            bib[tagName] = "";
                        }
                        break;

                    case ParserState.InTagValue:
                        if (TryMatch(token, TokenType.String))
                        {
                            tagValueBuilder.Append(token.Value);
                        }
                        break;

                    case ParserState.OutTagValue:
                        if(TryMatch(token, TokenType.Comma))
                        {
                            bib[tagName] = tagValueBuilder.ToString();
                            tagValueBuilder.Clear();
                        }
                        else if(TryMatch(token, TokenType.RightBrace))
                        {
                            bib[tagName] = tagValueBuilder.ToString();
                            tagValueBuilder.Clear();
                            // Add to result list
                        }
                        break;
                }

                curState = nextState;
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

        /// <summary>
        /// Try match a token.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool TryMatch(Token token, TokenType type)
        {
            return token.Type == type;
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
