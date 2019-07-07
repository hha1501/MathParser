using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MathParser.Expressions.Arguments;

namespace MathParser.Expressions
{
    public class BinaryExpression : Expression
    {
        public Expression Left { get; private set; }
        public Expression Right { get; private set; }

        public BinaryType OperatorType { get; private set; }

        public BinaryExpression(Expression left, Expression right, BinaryType type) : base(NodeType.Binary)
        {
            Left = left;
            Right = right;
            OperatorType = type;

            if (Left.ContainVariable || Right.ContainVariable)
            {
                ContainVariable = true;
            }

            Simplified = false;
        }

        public override EvaluationResult Evaluate(IContext context)
        {
            // Evaluate left and right.
            EvaluationResult leftResult = Left.Evaluate(context);

            if (leftResult.Success)
            {
                EvaluationResult rightResult = Right.Evaluate(context);

                if (rightResult.Success)
                {
                    return ApplyBinaryOperator(leftResult.Value, rightResult.Value);
                }
                else
                {
                    return EvaluationResult.NewError(rightResult.AdditionalInfo);
                }
            }
            else
            {
                return EvaluationResult.NewError(leftResult.AdditionalInfo);
            }
        }

        EvaluationResult ApplyBinaryOperator(double left, double right)
        {
            double result = 0;

            switch (OperatorType)
            {
                case BinaryType.Add:
                    result = left + right;
                    break;
                case BinaryType.Subtract:
                    result = left - right;
                    break;
                case BinaryType.Multiply:
                case BinaryType.ImpliedMultiply:
                    result = left * right;
                    break;
                case BinaryType.Divide:
                    result = left / right;
                    break;
                case BinaryType.Power:
                    result = Math.Pow(left, right);
                    break;
                default:
                    break;
            }

            return EvaluationResult.NewSuccess(result);
        }
    }
}