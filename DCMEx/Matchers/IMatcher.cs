using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMEx.Matchers
{
    interface IMatcher
    {
        bool isMatch(string chars, params string[] groups);
        string[] getGroups();
    }
}
