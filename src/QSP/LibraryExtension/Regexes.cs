using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace QSP.LibraryExtension
{
    public static class Regexes
    {
        public static string PatternMatchAny(IEnumerable<string> candidates)
        {
            if(candidates.Any() == false) throw new ArgumentException();

            bool containEmptyStr = false;
            var sb = new StringBuilder("(");
            
            foreach(var i in candidates)
            {
                if(i == "")
                {
                    containEmptyStr = true;
                }
                else
                {
                    sb.Append(i);
                    sb.Append('|');
                }
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(')');
            if(containEmptyStr) sb.Append('?');
            return sb.ToString();
        }
    }
}