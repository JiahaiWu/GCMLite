using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCMEx
{
    class GroupFetcher
    {
        public GroupFetcher(string fetchTag)
        {
            this.groupFetchTag = fetchTag.Trim();
            if (groupFetchTag.Length > 0)
            {
                if (groupFetchTag.ElementAt(0).Equals(GroupTag.FetchTag))
                {
                    Int32.TryParse(groupFetchTag.Substring(1), out fetchIndex);
                }
                else
                {
                    throw new DCMExException("Synax error in indentify group tag.");
                }
            }
        }
        internal void tryFetchGroup(string[] groups, ref string checkChars)
        {
            if (fetchIndex > -1 && fetchIndex < groups.Length)
                checkChars= groups[fetchIndex];
        }
        public string groupFetchTag = null;
        public int fetchIndex = -1;
    }
}
