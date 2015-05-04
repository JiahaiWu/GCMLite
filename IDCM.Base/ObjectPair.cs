using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCM.Base
{
    public class ObjectPair<T, R>
    {
        public ObjectPair(T _key, R _val)
        {
            this.key = _key;
            this.val = _val;
        }

        private T key;

        public T Key
        {
            get { return key; }
            set { key = value; }
        }
        private R val;

        public R Val
        {
            get { return val; }
            set { val = value; }
        }

    }
}
