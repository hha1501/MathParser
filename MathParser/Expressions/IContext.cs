using System;
using System.Collections.Generic;
using System.Text;

namespace MathParser.Expressions
{
    public interface IContext
    {
        T GetComponent<T>();
        void AddComponent<T>(T component);
    }
}
