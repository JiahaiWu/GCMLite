using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCM.Base.AbsInterfaces
{
    public interface IMsgObserver
    {
        bool bind(IMsgListener listener);
        bool unbind(IMsgListener listener);
        int size();
    }
}
