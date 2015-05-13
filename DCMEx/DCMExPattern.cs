using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCMEx.Matchers;

namespace DCMEx
{
    class DCMExPattern
    {
        #region Members
       
        private PrefixToken prefix = PrefixToken.DefaultMode;
        private IMatcher matchers = null;
        private char[] expChars;
        private int cursor;
        #endregion

        #region Constructor&Destructor

        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <param name="expChars">NULL Valid</param>
        public DCMExPattern(char[] expChars)
        {
            this.expChars = expChars;
            cursor = 0;
            resetPrefixToken();
            if (prefix.Equals(PrefixToken.RegexOnly))
            {
                matchers = new DCMRegexMatcher(expChars, cursor, expChars.Length);
            }
            else
            {
                matchers = new DCMMatcherGroup(expChars, cursor, expChars.Length);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 设置前导符值
        /// </summary>
        private void resetPrefixToken()
        {
            if (this.expChars == null || this.expChars.Length < 1)
            {
                prefix = PrefixToken.Deprecated;
                cursor = 0;
            }
            else
            {
                switch (this.expChars[0])
                {
                    case (char)PrefixToken.RegexOnly:
                        prefix = PrefixToken.RegexOnly;
                        cursor = 1;
                        break;
                    case (char)PrefixToken.Deprecated:
                        prefix = PrefixToken.Deprecated;
                        cursor = 1;
                        break;
                    default:
                        prefix = PrefixToken.DefaultMode;
                        cursor = 0;
                        break;
                }
            }
        }
        public bool isMatch(string chars)
        {
            if (matchers != null)
                return matchers.isMatch(chars);
            return false;
        }
        #endregion

    }
}
