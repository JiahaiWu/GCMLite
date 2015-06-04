using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMEx.Matchers
{
    class DCMURIMatcher : IMatcher
    {
        private string formatPattern = null;
        public string groupFetchTag = null;
        List<string> groups = new List<string>();

        public DCMURIMatcher(char[] expChars, int from, int end)
        {
            string formatPattern = new String(expChars, from, end - from);
        }
        public bool isMatch(string chars, params string[] groups)
        {
            return true;
        }
        public void setGroupFetchTag(string groupFetchTag)
        {
            this.groupFetchTag = groupFetchTag;
        }
        public string[] getGroups()
        {
            return groups.ToArray();
        }
    }
}
