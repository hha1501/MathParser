using System;

namespace MathParser.Expressions.Builder
{
    public class ErrorInfo
    {
        const string ExpectOperatorMessage = "Operator expected";
        const string ExpectRightParenMessage = "Right parenthesis ) expected";
        const string ExpectLeftParenMessage = "Left parenthesis ( expected";
        const string ExpectExpressionMessage = "Expression expected";
        const string UnexpectRightParenMessage = "Unexpected right parenthesis )";

        static ErrorInfo()
        {
            ExpectOperator = new ErrorInfo { Message = ExpectOperatorMessage };
            ExpectRightParen = new ErrorInfo { Message = ExpectRightParenMessage };
            ExpectLeftParen = new ErrorInfo { Message = ExpectLeftParenMessage };
            ExpectExpression = new ErrorInfo { Message = ExpectExpressionMessage };
            UnexpectRightParen = new ErrorInfo { Message = UnexpectRightParenMessage };
        }

        public string Message { get; private set; }

        private ErrorInfo()
        {
        }

        public static ErrorInfo ExpectOperator;
        public static ErrorInfo ExpectRightParen;
        public static ErrorInfo ExpectLeftParen;
        public static ErrorInfo ExpectExpression;
        public static ErrorInfo UnexpectRightParen;
    }
}
