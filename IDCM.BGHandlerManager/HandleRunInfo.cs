﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCM.Base.ComPO
{
    public class HandleRunInfo
    {
        public HandleRunInfo()
        {
        }
        public HandleRunInfo(string name, string status, long runningtime = 0)
        {
            this.HName = name;
            this.Status = status;
            this.RunTime = runningtime;
        }
        public string HName { get; set; }
        public string Status { get; set; }
        public long RunTime { get; set; }
        public string Description { get; set; }
        public string handleType { get; set; }
    }
}
