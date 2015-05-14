using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMEx
{
    public sealed class DCMEx
    {
        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <param name="exp">NULL Valid</param>
        public DCMEx(string exp)
        {
            if (exp == null)
                exPattern = new DCMExPattern(null);
            char[] expChars = exp.ToCharArray();
            exPattern = new DCMExPattern(expChars);
        }
        public bool isMatch(string str)
        {
            return exPattern.isMatch(str);
        }
        private DCMExPattern exPattern = null;
    }
}
