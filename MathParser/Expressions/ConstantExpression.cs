using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

using MathParser.Expressions.Arguments;

namespace MathParser.Expressions
{
    public class ConstantExpression : Expression
    {
        public double Value { get; private set; }

        public ConstantExpression(double value) : base(NodeType.Constant)
        {
            Value = value;

            ContainVariable = false;
            Simplified = true;
        }

        public override EvaluationResult Evaluate(IContext context)
        {
            return EvaluationResult.NewSuccess(Value);
        }
    }
}