using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMEx.Matchers
{
    class DCMNumberMatcher : IMatcher
    {
        private string formatPattern = null;

        List<string> groups = new List<string>();

        public DCMNumberMatcher(char[] expChars, int from, int end)
        {
            string formatPattern = new String(expChars, from, end - from);
        }
        public bool isMatch(string chars, params string[] groups)
        {
            Decimal decial;
            this.groups.Add(chars);
            return Decimal.TryParse(chars, out decial);
        }
        public string[] getGroups()
        {
            return groups.ToArray();
        }
    }
}
