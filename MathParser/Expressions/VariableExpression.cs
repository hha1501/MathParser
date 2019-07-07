using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MathParser.Expressions.Arguments;

namespace MathParser.Expressions
{
    public class VariableExpression : Expression
    {
        public string Name { get; private set; }

        public VariableExpression(string name) : base(NodeType.Variable)
        {
            Name = name;
            ContainVariable = true;
        }

        public override EvaluationResult Evaluate(IContext context)
        {
            if (context == null)
            {
                return EvaluationResult.NewError(EvaluationResult.MissingContext);
            }

            ArgumentProvider argumentProvider = context.GetComponent<ArgumentProvider>();

            if (argumentProvider != null)
            {
                // Retrieve argument's value.
                if (argumentProvider.GetValue(Name, out double argValue))
                {
                    return EvaluationResult.NewSuccess(argValue);
                } 
            }

            return EvaluationResult.NewError(EvaluationResult.MissingArguments);
        }
    }
}