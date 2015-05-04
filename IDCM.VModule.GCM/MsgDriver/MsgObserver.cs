using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.Base.AbsInterfaces;
using System.Collections;
using System.Collections.Concurrent;

namespace IDCM.MsgDriver
{
    internal class MsgObserver:IMsgObserver
    {
        public bool bind(IMsgListener listener)
        {
            if (listener == null)
                return false;
            return listenerNoters.TryAdd(listener.GetType().FullName, listener);
        }
        public bool unbind(IMsgListener listener)
        {
            if (listener == null)
                return false;
            IMsgListener ls = null;
            return listenerNoters.TryRemove(listener.GetType().FullName, out ls);
        }
        public int size()
        {
            return listenerNoters.Count;
        }
        internal void reset()
        {
            ConcurrentDictionary<string, IMsgListener> ols = listenerNoters;
            listenerNoters = new ConcurrentDictionary<string, IMsgListener>();
            ols.Clear();
        }
        internal ICollection<IMsgListener> getMsgListeners()
        {
            return listenerNoters.Values;
        }
        private ConcurrentDictionary<string,IMsgListener> listenerNoters = new ConcurrentDictionary<string,IMsgListener>();
    }
}
