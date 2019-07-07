using System;

namespace MathParser.Expressions
{
    public class EvaluationResult
    {
        public const string MissingArguments = "Arguments missing.";
        public const string DivideZero = "Divide by zero.";
        public const string UnsupportedParameterCount = "Unsupported number of parameters.";
        public const string MissingContext = "Context missing.";

        public static EvaluationResult NewSuccess(double value)
        {
            return new EvaluationResult
            {
                Success = true,
                Value = value
            };
        }
        public static EvaluationResult NewError(params string[] info)
        {
            return new EvaluationResult
            {
                Success = false,
                AdditionalInfo = info 
            };
        }

        public double Value { get; private set; }

        public bool Success { get; private set; }
        public string[] AdditionalInfo { get; private set; }
    }
}
