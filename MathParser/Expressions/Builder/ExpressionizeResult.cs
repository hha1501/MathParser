using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathParser.Expressions;

namespace MathParser.Expressions.Builder
{
    public class ExpressionizeResult
    {

        public static ExpressionizeResult NewSuccess(Expression expression)
        {
            return new ExpressionizeResult
            {
                Success = true,
                CalculatedExpression = expression
            };
        }
        public static ExpressionizeResult NewError(ErrorInfo error)
        {
            return new ExpressionizeResult
            {
                Success = false,
                Error = error
            };
        }

        public Expression CalculatedExpression { get; private set; }

        public bool Success { get; private set; }
        public ErrorInfo Error { get; private set; }
    }
}
