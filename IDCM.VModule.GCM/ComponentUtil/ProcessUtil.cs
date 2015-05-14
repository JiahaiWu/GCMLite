using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace IDCM.ComponentUtil
{
    class ProcessUtil
    {
        /// <summary>
        /// 查询同一目录下是否存在已经运行的进程实例
        /// </summary>
        /// <returns></returns>
        public static Process checkDuplicateProcess()
        {
            try
            {
                Process currentProcess = Process.GetCurrentProcess(); //获取当前进程 
                //获取当前运行程序完全限定名 
                string currentFileName = currentProcess.MainModule.FileName;
                //获取进程名为ProcessName的Process数组。 
                Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);
                //遍历有相同进程名称正在运行的进程 
                foreach (Process process in processes)
                {
                    if (process.MainModule.FileName == currentFileName)
                    {
                        if (process.Id != currentProcess.Id) //根据进程ID排除当前进程 
                        {
                            //当前目录存在已运行的进程实例
                            return process;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Error in checkDuplicateProcess.",ex);
            }
            return null;
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
