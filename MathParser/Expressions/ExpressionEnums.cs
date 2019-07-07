using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathParser.Expressions
{
    public enum NodeType
    {
        Binary,
        Unary,
        Variable,
        Constant,
        Function
    }

    public enum BinaryType
    {
        Add,
        Subtract,
        Multiply,
        ImpliedMultiply,
        Divide,
        Power
    }
    public enum UnaryType
    {
        Positize,
        Negate
    }
}
