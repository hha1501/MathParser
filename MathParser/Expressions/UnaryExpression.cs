using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MathParser.Expressions.Arguments;

namespace MathParser.Expressions
{
    public class UnaryExpression : Expression
    {
        public Expression Child { get; private set; }

        public UnaryType OperatorType { get; private set; }

        public UnaryExpression(Expression child, UnaryType type) : base(NodeType.Unary)
        {
            Child = child;
            OperatorType = type;

            if (child.ContainVariable)
            {
                ContainVariable = true;
            }
        }

        public override EvaluationResult Evaluate(IContext context)
        {
            EvaluationResult childResult = Child.Evaluate(context);

            if (childResult.Success)
            {
                return ApplyUnaryOperator(childResult.Value);
            }
            else
            {
                return childResult;
            }
        }

        private EvaluationResult ApplyUnaryOperator(double value)
        {
            double result = value;

            if (OperatorType == UnaryType.Negate)
            {
                result = -result;
            }

            return EvaluationResult.NewSuccess(result);
        }
    }
}