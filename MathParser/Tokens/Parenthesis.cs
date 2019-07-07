using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathParser.Tokens
{
    public class Parenthesis : Token
    {
        #region Static factory methods
        public static Parenthesis NewLeft()
        {
            return new Parenthesis { Type = TokenType.ParenLeft };
        }
        public static Parenthesis NewRight()
        {
            return new Parenthesis { Type = TokenType.ParenRight };
        }
        #endregion

        private Parenthesis()
        {
        }

        protected override string GetStringRepresentation()
        {
            switch (Type)
            {
                case TokenType.ParenLeft:
                    return "(";
                case TokenType.ParenRight:
                    return ")";
                default:
                    return "";
            }
        }
    }
}