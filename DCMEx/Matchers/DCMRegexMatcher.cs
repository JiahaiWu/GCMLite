using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DCMEx.Matchers
{
    class DCMRegexMatcher : IMatcher
    {
        private Regex regex = null;
        private Match match = null;
        List<string> groups = new List<string>();


        public DCMRegexMatcher(char[] expChars, int from, int end)
        {
            string pattern = new String(expChars, from, end - from);
            regex = new Regex(pattern);
        }
        public bool isMatch(string chars, params string[] groups)
        {
            match = regex.Match(chars);
            return match.Success;
        }
        public string[] getGroups()
        {
            foreach (Group grc in match.Groups)
            {
                groups.Add(grc.Value);
            }
            return match != null ? groups.ToArray() : null;
        }
    }
}
