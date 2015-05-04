using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCM.Base.Utils
{
    public interface Base64SharedI
    {
        char charAt(int index);
        int indexOf(char ch);
        char pad();
    }
}
