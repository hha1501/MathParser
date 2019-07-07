using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathParser.Tokens
{

    public class FunctionHeader : Token
    {
        #region Static factory methods
        public static FunctionHeader New(string name, string target, int numberOfArguments)
        {
            return new FunctionHeader(name, target, numberOfArguments);
        }
        #endregion

        public string Name { get; private set; }
        public string Target { get; private set; }

        public int ArgumentCount { get; private set; }

        private FunctionHeader(string name, string target, int numberOfArguments)
        {
            Type = TokenType.Function;

            Name = name;
            Target = target;
            ArgumentCount = numberOfArguments;
        }

        protected override string GetStringRepresentation()
        {
            return Name;
        }
    }
}
