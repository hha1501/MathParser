using System;
using System.Linq;

using MathParser.Tokens;
using MathParser.Tokens.Tokenizing;
using MathParser.Expressions;
using MathParser.Expressions.Builder;

namespace MathParserRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = String.Empty;
            string output;

            while (input != "exit")
            {
                input = Console.ReadLine();

                Tokenizer tokenizer = new Tokenizer();
                TokenizeResult tokenizeResult = tokenizer.TokenizeString(input);

                if (!tokenizeResult.Success)
                {
                    output = tokenizeResult.AddtionalInfo;
                }
                else
                {
                    ExpressionBuilder expressionBuilder = new ExpressionBuilder(tokenizeResult.TokenList);
                    ExpressionizeResult expressionizeResult = expressionBuilder.GenerateExpression();

                    if (!expressionizeResult.Success)
                    {
                        output = expressionizeResult.Error.Message;
                    }
                    else
                    {
                        EvaluationResult evaluationResult = expressionizeResult.CalculatedExpression.Evaluate(null);

                        if (!evaluationResult.Success)
                        {
                            output = evaluationResult.AdditionalInfo.Aggregate((item, aggregator) => $"{aggregator}\n{item}");
                        }
                        else
                        {
                            output = evaluationResult.Value.ToString();
                        }
                    }
                }

                Console.WriteLine(output); 
            }
        }
    }
}
