using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathParser.Tokens;
using MathParser.Expressions;

namespace MathParser.Expressions.Extensions
{
    public static class ExpressionExtensions
    {
        public static bool IsLeftAssociative(this BinaryType operatorType)
        {
            switch (operatorType)
            {
                case BinaryType.Add:
                case BinaryType.Subtract:
                case BinaryType.Multiply:
                case BinaryType.Divide:
                case BinaryType.ImpliedMultiply:
                    return true;
                case BinaryType.Power:
                    return false;

                default:
                    return true;
            }
        }

        public static int GetPrecedence(this BinaryType operatorType)
        {
            switch (operatorType)
            {
                case BinaryType.Add:
                case BinaryType.Subtract:
                    return 1;
                case BinaryType.Multiply:
                case BinaryType.Divide:
                    return 2;
                case BinaryType.ImpliedMultiply:
                    return 3;
                case BinaryType.Power:
                    return 4;

                default:
                    return 1;
            }
        }
        public static int GetPrecedence(this UnaryType opartorType)
        {
            return GetPrecedence(BinaryType.Add);
        }

        public static BinaryType ToBinaryType(this TokenType operatorType)
        {
            switch (operatorType)
            {
                case TokenType.OpAdd:
                    return BinaryType.Add;

                case TokenType.OpSubtract:
                    return BinaryType.Subtract;

                case TokenType.OpMultiply:
                    return BinaryType.Multiply;

                case TokenType.OpDivide:
                    return BinaryType.Divide;

                case TokenType.OpPower:
                    return BinaryType.Power;


                default:
                    return BinaryType.Add;
            }
        }
        public static UnaryType ToUnaryType(this TokenType operatorType)
        {
            switch (operatorType)
            {
                case TokenType.OpAdd:
                    return UnaryType.Positize;
                case TokenType.OpSubtract:
                    return UnaryType.Negate;

                default:
                    return UnaryType.Positize;
            }
        }

        public static string ToStringRepresentation(this BinaryType binaryType)
        {
            switch (binaryType)
            {
                case BinaryType.Add:
                    return "+";
                case BinaryType.Subtract:
                    return "-";
                case BinaryType.Multiply:
                    return "*";
                case BinaryType.ImpliedMultiply:
                    return "*";
                case BinaryType.Divide:
                    return "/";
                case BinaryType.Power:
                    return "^";
                default:
                    return "";
            }
        }
        public static string ToStringRepresentation(this UnaryType unaryType)
        {
            switch (unaryType)
            {
                case UnaryType.Positize:
                    return "+";
                case UnaryType.Negate:
                    return "-";
                default:
                    return "";
            }
        }
    }
}
