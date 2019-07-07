using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathParser.Expressions.Arguments
{
    public class ArgumentProvider
    {
        private Dictionary<string, double> argumentsDictionary;

        public ArgumentProvider()
        {
            argumentsDictionary = new Dictionary<string, double>();
        }

        public void Add(string argumentName, double value)
        {
            argumentsDictionary[argumentName] = value;
        }

        public bool GetValue(string argumentName, out double value)
        {
            if (argumentsDictionary.ContainsKey(argumentName))
            {
                value = argumentsDictionary[argumentName];
                return true;
            }

            value = 0;
            return false;
        }
    }
}
