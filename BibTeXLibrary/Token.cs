using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibTeXLibrary
{
    internal struct Token
    {
        public TokenType Type;
        public string Value;

        public Token(TokenType type, string value = "")
        {
            Type = type;
            Value = value;
        }
    }

    enum TokenType
    {
        Start,
        
        String,
        Number,

        Quotation,

        LeftBrace,
        RightBrace,

        Comma,

        Concatenation
    }
}
