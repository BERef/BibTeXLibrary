using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Debug.WriteLine(type.ToString() + "\t" + value);
        }
    }

    public enum TokenType
    {
        Start,
        
        Name,
        String,

        Quotation,

        LeftBrace,
        RightBrace,

        Equal,
        Comma,

        Concatenation,

        EOF
    }
}
