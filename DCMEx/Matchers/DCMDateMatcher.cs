using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMEx.Matchers
{
    class DCMDateMatcher : IMatcher
    {
        private string formatPattern = null;

        List<string> groups = new List<string>();

        public DCMDateMatcher(char[] expChars, int from, int end)
        {
            string formatPattern = new String(expChars, from, end - from);
        }
        public bool isMatch(string chars, params string[] groups)
        {
            DateTime dt = DateTime.Now;
            this.groups.Add(chars);
            return DateTime.TryParse(chars, out dt);
        }
        public string[] getGroups()
        {
            return groups.ToArray();
        }
    }
}
