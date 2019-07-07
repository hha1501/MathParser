using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathParser.Tokens;

namespace MathParser.Tokens.Tokenizing
{
    public class ParseResult
    {
        public const string DecimalPoint = "Syntax error. A number has more than one decimal point.";
        public const string UnknownCharacter = "Unknown character.";
        public const string UnknownError = "Unknown error.";

        public static ParseResult NewSuccess(Token parsedToken, int tokenSize)
        {
            return new ParseResult
            {
                ParsedToken = parsedToken,
                TokenSize = tokenSize,
                Success = true
            };
        }
        public static ParseResult NewError(string errorInfo)
        {
            return new ParseResult
            {
                Success = false,
                AdditionalInfo = errorInfo
            };
        }

        public bool Success { get; set; }
        public string AdditionalInfo { get; set; }

        public Token ParsedToken { get; set; }
        public int TokenSize { get; set; }
    }

}
