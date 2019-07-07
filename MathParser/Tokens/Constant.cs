using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathParser.Tokens
{
    public class Constant : Token
    {
        #region Static factory methods
        public static Constant New(double value)
        {
            return new Constant(value);
        }
        public static Constant New(string name, double value)
        {
            return new Constant(name, value);
        }
        #endregion

        public string Name { get; private set; }
        public bool HasName { get; private set; }

        public double Value { get; private set; }

        private Constant(double value)
        {
            Type = TokenType.Constant;

            HasName = false;

            Value = value;
        }
        private Constant(string name, double value)
        {
            Type = TokenType.Constant;

            Name = name;
            HasName = true;

            Value = value;
        }

        protected override string GetStringRepresentation()
        {
            return HasName ? Name : Value.ToString();
        }
    }
}