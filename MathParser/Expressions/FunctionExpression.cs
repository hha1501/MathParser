using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using MathParser.Expressions.Arguments;

namespace MathParser.Expressions
{
    public class FunctionExpression : Expression
    {
        public string Name { get; private set; }
        public MethodInfo Target { get; private set; }

        public List<Expression> ArgumentsList { get; private set; }


        public FunctionExpression(string name, string target, List<Expression> argumentsList) : base(NodeType.Function)
        {
            Name = name;
            ArgumentsList = argumentsList;

            Target = typeof(Math).GetMethod(target);
            if (Target == null)
            {
                throw new NotImplementedException($"{nameof(target)} not found.");
            }

            ContainVariable = argumentsList.Any(argExpression => argExpression.ContainVariable);
            Simplified = false;
        }

        public override EvaluationResult Evaluate(IContext context)
        {
            // Evaluate all arguments.
            double[] evaluatedArguments = new double[ArgumentsList.Count];

            for (int i = 0; i < ArgumentsList.Count; i++)
            {
                EvaluationResult evaluationResult = ArgumentsList[i].Evaluate(context);

                if (!evaluationResult.Success)
                {
                    return evaluationResult;
                }
                else
                {
                    evaluatedArguments[i] = evaluationResult.Value;
                }
            }

            return ApplyFunction(evaluatedArguments);
        }

        private EvaluationResult ApplyFunction(double[] arguments)
        {
            object[] objArguments = new object[arguments.Length];
            for (int i = 0; i < arguments.Length; i++)
            {
                objArguments[i] = arguments[i];
            }

            double result;

            try
            {
                result = (double)Target.Invoke(null, objArguments);
            }
            catch (TargetParameterCountException)
            {
                return EvaluationResult.NewError(EvaluationResult.UnsupportedParameterCount);
            }

            return EvaluationResult.NewSuccess(result);
        }
    }
}
