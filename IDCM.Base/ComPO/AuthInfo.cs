using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCM.Base.ComPO
{
    public class AuthInfo
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Jsessionid { get; set; }
        public bool LoginFlag { get; set; }
        public bool autoLogin { get; set; }
        public long Timestamp { get; set; }
    }
}
