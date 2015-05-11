using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMEx.Matchers
{
    class DCMMatcherFactory
    {
        internal static IMatcher getMatcher(PatternMode pMode, char[] expChars, int from, int end)
        {
            switch (pMode)
            {
                case PatternMode.COMPLEX:
                    return new DCMMatcherGroup(expChars, from, end);
                case PatternMode.REGEX:
                    return new DCMRegexMatcher(expChars, from, end);
                case PatternMode.DATE:
                    return new DCMDateMatcher(expChars, from, end);
                case PatternMode.NUMBER:
                    return new DCMNumberMatcher(expChars, from, end);
                case PatternMode.FILE:

                default:
                    throw new DCMExException("unsupported mode!");
            }
        }
    }
}
