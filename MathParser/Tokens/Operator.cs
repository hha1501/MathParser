using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathParser.Tokens
{
    public class Operator : Token
    {
        #region Static factory methods
        public static Operator NewAddition()
        {
            return new Operator { Type = TokenType.OpAdd };
        }
        public static Operator NewSubtraction()
        {
            return new Operator { Type = TokenType.OpSubtract };
        }
        public static Operator NewMultiplication()
        {
            return new Operator { Type = TokenType.OpMultiply};
        }
        public static Operator NewDivision()
        {
            return new Operator { Type = TokenType.OpDivide };
        }
        public static Operator NewPower()
        {
            return new Operator { Type = TokenType.OpPower };
        }
        #endregion

        private Operator()
        {
        }

        protected override string GetStringRepresentation()
        {
            switch (Type)
            {
                case TokenType.OpAdd:
                    return "+";
                case TokenType.OpSubtract:
                    return "-";
                case TokenType.OpMultiply:
                    return "*";
                case TokenType.OpDivide:
                    return "/";
                case TokenType.OpPower:
                    return "^";
                default:
                    return "";
            }
        }
    }
}