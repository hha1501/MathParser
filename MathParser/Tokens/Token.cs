using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathParser.Tokens
{
    public abstract class Token
    {
        public TokenType Type { get; protected set; }
        public string StringRepresentation => GetStringRepresentation();

        protected virtual string GetStringRepresentation()
        {
            return Type.ToString();
        }

        public override string ToString()
        {
            return GetStringRepresentation();
        }
    }
}
