using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace BibTeXLibrary
{
	using Action	= Dictionary<TokenType, Tuple<ParserState, BibBuilderState>>;
	using Next		= Tuple<ParserState, BibBuilderState>;
	using StateMap	= Dictionary<ParserState, Dictionary<TokenType, Tuple<ParserState, BibBuilderState>>>;

	public sealed class BibParser : IDisposable
    {
		#region Static Fields

		public static char[] _beginCommentCharacters = { '%' };

		#endregion

		#region Constant Fields

		/// <summary>
		/// State tranfer map
		/// curState --Token--> (nextState, BibBuilderAction)
		/// </summary>
		private static readonly StateMap StateMap = new StateMap
        {
            {ParserState.Begin,       new Action {
                { TokenType.Comment,       new Next(ParserState.InHeader,    BibBuilderState.SetHeader) }, 
                { TokenType.Start,         new Next(ParserState.InStart,     BibBuilderState.Create) }
            } },

			{ParserState.InHeader,    new Action {
				{ TokenType.Comment,       new Next(ParserState.InHeader,    BibBuilderState.SetHeader) },
				{ TokenType.Start,         new Next(ParserState.InStart,     BibBuilderState.Create) }
			} },

			{ParserState.InStart,     new Action {
                { TokenType.Name,          new Next(ParserState.InEntry,     BibBuilderState.SetType) }
            } },

            {ParserState.InEntry,     new Action {
                { TokenType.LeftBrace,     new Next(ParserState.InKey,       BibBuilderState.Skip) }
            } },

            {ParserState.InKey,       new Action {
                { TokenType.RightBrace,    new Next(ParserState.OutEntry,    BibBuilderState.Build) },
                { TokenType.Name,          new Next(ParserState.OutKey,      BibBuilderState.SetKey) },
                { TokenType.String,        new Next(ParserState.OutKey,      BibBuilderState.SetKey) },
                { TokenType.Comma,         new Next(ParserState.InTagName,   BibBuilderState.Skip) }
			} },

            {ParserState.OutKey,      new Action {
                { TokenType.Comma,         new Next(ParserState.InTagName,   BibBuilderState.Skip) }
            } },

            {ParserState.InTagName,   new Action {
                { TokenType.Name,          new Next(ParserState.InTagEqual,  BibBuilderState.SetTagName) },
                { TokenType.RightBrace,    new Next(ParserState.OutEntry,    BibBuilderState.Build) }
            } },

            {ParserState.InTagEqual,  new Action {
                { TokenType.Equal,         new Next(ParserState.InTagValue,  BibBuilderState.Skip) }
             } },

            {ParserState.InTagValue,  new Action {
                { TokenType.String,        new Next(ParserState.OutTagValue, BibBuilderState.SetTagValue) },
                { TokenType.Name,          new Next(ParserState.OutTagValue, BibBuilderState.SetTagValue) }
            } },

            {ParserState.OutTagValue, new Action {
                { TokenType.Concatenation, new Next(ParserState.InTagValue,  BibBuilderState.Skip) },
                { TokenType.Comma,         new Next(ParserState.InTagName,   BibBuilderState.SetTag) },
                { TokenType.RightBrace,    new Next(ParserState.OutEntry,    BibBuilderState.Build) }
             } },

            {ParserState.OutEntry,    new Action {
                { TokenType.Start,         new Next(ParserState.InStart,     BibBuilderState.Create) },
                { TokenType.Comment,       new Next(ParserState.InComment,   BibBuilderState.Skip) }, 
            } },

            {ParserState.InComment,    new Action {
                { TokenType.Start,         new Next(ParserState.InStart,     BibBuilderState.Create) },
                { TokenType.Comment,       new Next(ParserState.InComment,   BibBuilderState.Skip) }, 
            } },
		};

        #endregion

        #region Private Fields

        /// <summary>
        /// Input text stream.
        /// </summary>
        private readonly TextReader _inputText;

        /// <summary>
        /// Line No. counter.
        /// </summary>
        private int _lineCount = 1;

        /// <summary>
        /// Column counter.
        /// </summary>
        private int _colCount;

        /// <summary>
        /// File header.
        /// </summary>
        private List<string> _header								= new List<string>();

        /// <summary>
        /// Initializer for BibEntrys.  Used  to allow a defined order of tags.
        /// </summary>
        private BibEntryInitialization _bibEntryInitialization		= new BibEntryInitialization();

		#endregion

		#region Public Fields

		/// <summary>
		/// Initializer for BibEntrys.  Used  to allow a defined order of tags.
		/// </summary>
		public BibEntryInitialization BibEntryInitializer { get => _bibEntryInitialization; set => _bibEntryInitialization = value; }

        /// <summary>
        /// Bibliography header text.
        /// </summary>
		public List<string> Header { get => _header; set => _header = value; }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor that reads a file using a StreamReader with default encoding.
		/// </summary>
		/// <param name="path">Full path and file name to the file to reader.</param>
		public BibParser(string path) :
            this(new StreamReader(path, Encoding.Default))
		{
		}

		/// <summary>
		/// Constructor that reads a file using a StreamReader with default encoding.
		/// </summary>
		/// <param name="path">Full path and file name to the file to reader.</param>
		public BibParser(string path, BibEntryInitialization bibEntryInitialization) :
			this(new StreamReader(path, Encoding.Default), bibEntryInitialization)
		{
		}

		/// <summary>
		/// Constructor with a reader.
		/// </summary>
		/// <param name="textReader">TextReader.</param>
		public BibParser(TextReader textReader)
        {
            _inputText = textReader;
        }

		/// <summary>
		/// Constructor with a reader.
		/// </summary>
		/// <param name="textReader">TextReader.</param>
		public BibParser(TextReader textReader, BibEntryInitialization bibEntryInitialization)
		{
			_inputText              = textReader;
            _bibEntryInitialization = bibEntryInitialization;
		}


		#endregion

		#region Public Static Methods

		/// <summary>
		/// Parse by given input text reader.
		/// </summary>
		/// <param name="inputText"></param>
		public static Tuple<List<string>, List<BibEntry>> Parse(TextReader inputText)
        {
            using (BibParser parser = new BibParser(inputText))
            {
				List<BibEntry> entries = parser.Parse().ToList();
				return new Tuple<List<string>, List<BibEntry>>(parser._header, entries);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get all results from the Parser.
        /// </summary>
        public Tuple<List<string>, List<BibEntry>> GetAllResults()
        {
			List<BibEntry> entries = Parse().ToList();
			return new Tuple<List<string>, List<BibEntry>>(_header, entries);
        }

		#endregion

		#region Private Methods

		private IEnumerable<BibEntry> Parse()
        {
            try
            {
				ParserState		curState			= ParserState.Begin;
				ParserState		nextState			= ParserState.Begin;

                BibEntry		bibEntry			= null;
                string			tagName				= "";
				bool			tagValueIsString	= false;
				StringBuilder	tagValueBuilder		= new StringBuilder();

                // Fetch token from Tokenizer and build BibEntry.
                foreach (Token token in Tokenize())
                {
                    // Transfer state
                    if (StateMap[curState].ContainsKey(token.Type))
					{
						nextState = StateMap[curState][token.Type].Item1;
					}
					else
					{
                        var expected = from pair in StateMap[curState] select pair.Key;
						throw new UnexpectedTokenException(_lineCount, _colCount, token.Type, expected.ToArray());
                    }
                    // Build BibEntry
                    switch (StateMap[curState][token.Type].Item2)
                    {
                        case BibBuilderState.SetHeader:
                            _header.Add(token.Value);
                            break;

                        case BibBuilderState.Create:
                            bibEntry = new BibEntry();
                            break;

                        case BibBuilderState.SetType:
                            Debug.Assert(bibEntry != null, "bib != null");
                            bibEntry.Type = token.Value;
                            bibEntry.Initialize(_bibEntryInitialization.GetTags(bibEntry));
							break;

                        case BibBuilderState.SetKey:
                            Debug.Assert(bibEntry != null, "bib != null");
                            bibEntry.Key = token.Value;
                            break;

                        case BibBuilderState.SetTagName:
                            tagName = token.Value;
                            break;

                        case BibBuilderState.SetTagValue:
							if (token.Type != TokenType.Concatenation)
							{
								tagValueIsString = token.Type == TokenType.String;
							}
							tagValueBuilder.Append(token.Value);
                            break;

                        case BibBuilderState.SetTag:
                            Debug.Assert(bibEntry != null, "bib != null");
							bibEntry.SetTagValue(tagName, new TagValue(tagValueBuilder.ToString(), tagValueIsString));
                            tagValueBuilder.Clear();
                            tagName = string.Empty;
                            break;

                        case BibBuilderState.Build:
                            if (tagName != string.Empty)
                            {
                                Debug.Assert(bibEntry != null, "bib != null");
                                bibEntry[tagName] = tagValueBuilder.ToString();
                                tagValueBuilder.Clear();
                                tagName = string.Empty;
                            }
                            yield return bibEntry;
                            break;
                    }
                    curState = nextState;
                }
                if (curState != ParserState.OutEntry)
                {
                    var expected = from pair in StateMap[curState] select pair.Key;
                    throw new UnexpectedTokenException(_lineCount, _colCount, TokenType.EOF, expected.ToArray());
                }
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// Tokenizer for BibTeX entry.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Token> Tokenize()
        {
            int     code;
            char    c;
            int     braceCount  = 0;

            while ((code = Peek()) != -1)
            {
                c = (char)code;

                if (c == '@')
                {                    
                    yield return new Token(TokenType.Start);
                }
                else if (IsStringCharacter(c))
                {
                    var value = new StringBuilder();

                    while (true)
                    {
                        c = (char)Read();
                        value.Append(c);

                        if ((code = Peek()) == -1) break;
                        c = (char)code;

						if (!IsStringCharacter(c)) break;
                    }
                    yield return new Token(TokenType.Name, value.ToString());
                    goto ContinueExcute;
                }
                else if (c == '"')
                {
					_inputText.Read();

					// Some entries have the unfortunate practice of using a start sequence of "{ and an end sequence of }".
					// BibTeX seems to allow this.  We will treat "{ as a single { and similar for the closing sequence.  There,
					// if we read "{, we ignore the quotation and continue.
					if (Peek() != '{')
					{
						StringBuilder value = new StringBuilder();
						while ((code = Peek()) != -1)
						{
							if (c != '\\' && code == '"') break;

							c = (char)Read();
							value.Append(c);

						}
						yield return new Token(TokenType.String, value.ToString());
					}
					else
					{
						goto ContinueExcute;
					}
				}
                else if (c == '{')
                {
                    if (braceCount++ == 0)
                    {
                        yield return new Token(TokenType.LeftBrace);
                    }
                    else
                    {
                        var value = new StringBuilder();
						// Read the brace (was only peeked).
                        Read();
                        while (braceCount > 1 && Peek() != -1)
                        {
                            c = (char)Read();
							if (c == '{')
							{
								braceCount++;
							}
							else if (c == '}')
							{
								braceCount--;
							}

							if (braceCount > 1)
							{
								value.Append(c);
							}
						}
                        yield return new Token(TokenType.String, value.ToString());

						// Ignore the quotation in a }" combination.
						if (Peek() == '"')
						{
							Read();
						}

                        goto ContinueExcute;
                    }
                }
                else if (c == '}')
                {
                    braceCount--;
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
                    _colCount = 0;
                    _lineCount++;
                }
                else if (_beginCommentCharacters.Any(item => item == c))
                {
                    _colCount = 0;
                    _lineCount++;
                    yield return new Token(TokenType.Comment, _inputText.ReadLine());
					goto ContinueExcute;
				}
                else if (!char.IsWhiteSpace(c))
                {
                    throw new UnrecognizableCharacterException(_lineCount, _colCount, c);
                }

				// Move to next char if possible.
				Read();

                // Don't move.
                ContinueExcute:;
            }
        }

		private bool IsStringCharacter(char c)
		{
			if (char.IsLetterOrDigit(c) ||
				c == '-' ||
				c == '.' ||
				c == '_' ||
				c == '—' ||
				c == ':' ||
				c == '/' ||
				c == '\\')
			{
				return true;
			}
			else
			{
				return false;
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
            _colCount++;
			if (_inputText.Peek() != -1)
			{
				return _inputText.Read();
			}
			else
			{
				return -1;
			}
        }

        #endregion

        #region Impement Interface "IDisposable"

        /// <summary>
        /// Dispose stream resource.
        /// </summary>
        public void Dispose()
        {
            _inputText?.Dispose();
        }

        #endregion

    } // End class.
} // End namespace.