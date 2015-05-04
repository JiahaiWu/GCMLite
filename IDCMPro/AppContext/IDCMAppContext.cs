using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using IDCM.VModule.GCM;
using IDCM.BGHandlerManager;
using IDCM.Base.Utils;
using IDCM.Base;
/********************************
 * Individual Data Center of Microbial resources (IDCM)
 * A desktop software package for microbial resources researchers.
 * 
 * Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
 * 
 * @Contact NO.1 Beichen West Road, Chaoyang District, Beijing 100101, Email: office@im.ac.cn
 */
namespace IDCM.AppContext
{
    /// <summary>
    /// The class that handles the creation of the application windows
    /// 本类作为IDCM的窗口任务、后台任务、线程任务的创建管理类。
    /// 本实例只允许一次有效实例化，启动初始化窗口线程和心跳检测任务。
    /// 心跳检测任务会定期地判断当前进程是否活动任务为空，准备退出。
    /// @author JiahaiWu 2014-10
    /// </summary>
    class IDCMAppContext : ApplicationContext
    {
        /// <summary>
        /// 初始化应用程序上下文环境，并触发程序主界面启动的事件过程。
        /// 说明：
        /// 1. 同一个工作空间下本实例进程要求排他性，如果已存在其他进程占用则执行退出过程。
        /// 2. IDCMFormManager为用户交互界面主控管理器实现，该实例对象保持唯一性；
        /// 3. 通过后台任务轮询监视器monitor来识别当前应用是否需要自动销毁进程资源，
        /// 对于长驻线程任务实现应当注册到RunningHandlerNoter实例中，以标志当前线程任务具有事务保护性。
        /// 由此实现的程序完全退出机制不受单一用户界面的关闭影响，强化事务完整性和数据操作完整性。
        /// 在诸项事务队列完成之后，才能有效退出。
        /// 4. 任务轮询监视器monitor的监听时间默认为2s。
        /// @author JiahaiWu 2014-12
        /// </summary>
        public IDCMAppContext(Dictionary<string, string> preSetttings=null)
        {
            if (!appInited)
            {
                appInited = true;
                // Handle the ApplicationExit event to know when the application is exiting.
                Application.ApplicationExit +=this.OnApplicationExit;
                // Create main application form and active the initForm method
                prepareGCMPro(preSetttings);
                mainManger = new GCMPro();
                mainManger.FormClosed+=this.OnMainManagerExit;
                mainManger.Show();
                //Run HandleInstanceMonitor
                handleMonitor.Interval = 500;
                handleMonitor.Elapsed += OnHMHeartBreak;
                handleMonitor.Start();
            }
        }
        /// <summary>
        /// 命令行参数预处理
        /// </summary>
        /// <param name="preActions"></param>
        private static void prepareGCMPro(Dictionary<string, string> setttings=null)
        {
            foreach (KeyValuePair<string, string> kvpair in setttings)
            {
                if (kvpair.Key.Equals(SysConstants.LastWorkSpace,StringComparison.CurrentCultureIgnoreCase))
                {
                    ConfigurationHelper.SetAppConfig(SysConstants.LastWorkSpace, kvpair.Value);
                }
            }
        }
        /// <summary>
        /// 应用程序常规退出触发事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationExit(object sender, EventArgs e)
        {
            log.Info("On IDCM application exiting.");
        }
        private void OnMainManagerExit(object sender, EventArgs e)
        {
            log.Info("On Main Manager exited.");
            mainManger = null;
        }

        /// <summary>
        /// 后台任务轮询监视器的心跳检测事件处理
        /// 说明：
        /// 当长驻线程任务实例悉数完成或中断时，满足IDCM应用进程退出的条件时立即销毁进程资源操作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHMHeartBreak(object sender, EventArgs e)
        {
            if (mainManger != null && appInited)
                return;
            if (RunningHandlerNoter.checkForIdle())
            {
                handleMonitor.Stop();
                ExitThread();
                this.Dispose();
            }
        }

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// HandleInstanceMonitor
        /// </summary>
        private static System.Timers.Timer handleMonitor = new System.Timers.Timer();
        private static volatile bool appInited = false;
        private static GCMPro mainManger = null;
    }
}