using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathParser.Tokens;

namespace MathParser.Tokens.Tokenizing
{
    public class TokenizeResult
    {
        public static TokenizeResult NewSuccess(List<Token> tokenList)
        {
            return new TokenizeResult
            {
                Success = true,
                TokenList = tokenList
            };
        }
        public static TokenizeResult NewError(string error)
        {
            return new TokenizeResult
            {
                Success = false,
                AddtionalInfo = error
            };
        }

        public bool Success { get; set; }
        public string AddtionalInfo { get; set; }

        public List<Token> TokenList { get; set; }
    }
}
