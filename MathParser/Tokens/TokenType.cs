using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathParser.Tokens
{
    public enum TokenType
    {
        Constant = 1 << 0,
        Variable = 1 << 1,

        ParenLeft = 1 << 2,
        ParenRight = 1 << 3,

        OpAdd = 1 << 4,
        OpSubtract = 1 << 5,
        OpMultiply = 1 << 6,
        OpDivide = 1 << 7,
        OpPower = 1 << 8,

        Function = 1 << 9,

        Paren = ParenLeft | ParenRight,

        BinaryOperator = OpAdd | OpSubtract | OpMultiply | OpDivide | OpPower,
        UnaryOperator = OpAdd | OpSubtract,
        Operator = OpAdd | OpSubtract | OpMultiply | OpDivide | OpPower
    }
}
