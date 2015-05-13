﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCM.BGHandlerManager
{
    internal class HandleRunStatus
    {
        #region Members
     
        /// <summary>
        /// 空闲状态
        /// </summary>
        public static readonly HandleRunStatus Idle = new HandleRunStatus("Idle", 0);
        /// <summary>
        /// 运行中状态
        /// </summary>
        public static readonly HandleRunStatus InWorking = new HandleRunStatus("InWorking", 1);
        /// <summary>
        /// 已结束状态
        /// </summary>
        public static readonly HandleRunStatus Finished = new HandleRunStatus("Finished", 2);
        /// <summary>
        /// 未知状态(具体可能是运行完成、运行取消或异常终止等情形)
        /// </summary>
        public static readonly HandleRunStatus Unknown = new HandleRunStatus("Unknown", 3);

        private readonly string name;
        private readonly double value;
        #endregion

        #region Constructor&Destructor

        HandleRunStatus(string name, int value)
        {
            this.name = name;
            this.value = value;
        }
        #endregion

        #region Property
        public string Name { get { return name; } }

        public double Value { get { return value; } }
        #endregion

        #region Methods
        public override string ToString()
        {
            return name;
        }

        /// <summary>
        /// For iterator 
        /// </summary>
        public static IEnumerable<HandleRunStatus> Values
        {
            get
            {
                yield return Idle;
                yield return InWorking;
                yield return Finished;
                yield return Unknown;
            }
        }
        #endregion
    }
}
