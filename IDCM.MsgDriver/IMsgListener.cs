using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCM.MsgDriver
{
    public interface IMsgListener
    {
        void reportJobProgress(Int32 percent);
        void reportSimpleMsg(DCMMessage dmsg);
        void reportJobFeedback(AsyncMsgNotice amsg);
    }
}
