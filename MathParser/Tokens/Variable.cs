using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathParser.Tokens
{
    public class Variable : Token
    {
        #region Static factory methods
        public static Variable New(string name)
        {
            return new Variable(name);
        }
        #endregion

        public string Name { get; private set; }

        private Variable(string name)
        {
            Type = TokenType.Variable;
            Name = name;
        }

        protected override string GetStringRepresentation()
        {
            return Name;
        }
    }
}