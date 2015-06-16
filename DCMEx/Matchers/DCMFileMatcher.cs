using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMEx.Matchers
{
    class DCMFileMatcher : IMatcher
    {
        private string formatPattern = null;
        public GroupFetcher groupFetchTag = null;
        List<string> groups = new List<string>();

        public DCMFileMatcher(char[] expChars, int from, int end)
        {
            string formatPattern = new String(expChars, from, end - from);
        }
        public bool isMatch(string chars, params string[] groups)
        {
            return true;
        }
        public void setGroupFetchTag(GroupFetcher groupFetchTag)
        {
            this.groupFetchTag = groupFetchTag;
        }
        public string[] getGroups()
        {
            return groups.ToArray();
        }
    }
}
