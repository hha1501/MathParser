using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathParser.Tokens
{
    public static class CharacterExtension
    {
        public static bool IsArabicDigit(this char c)
        {
            return (c >= '0' && c <= '9');
        }

        public static string GetString(this char[] charArray, int start)
        {
            return new string(charArray, start, charArray.Length - start);
        }
    }
}
