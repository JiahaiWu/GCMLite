using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.Base.ComPO;
using System.ComponentModel;
using System.Threading;

namespace IDCM.BGHandlerManager
{
    /// <summary>
    /// 内部线程实例化句柄统一监管器
    /// 说明：
    /// 1.用于明确进程内线程资源的跟踪记录与安全退出验证服务
    /// @author JiahaiWu 
    /// </summary>
    public class RunningHandlerNoter
    {
        #region Methods

        /// <summary>
        /// 获取当前后台线程对象的存活状态快照集
        /// </summary>
        /// <returns></returns>
        public static HandleRunInfo[] getHandleList()
        {
            List<HandleRunInfo> hris = new List<HandleRunInfo>();
            lock (BackendHandleMonitor_Lock)
            {
                foreach (HandleHolderI handle in handleList)
                {
                    if (handle == null)
                        continue;
                    HandleRunInfo hri = handle.ToHandleRunInfo();
                    hris.Add(hri);
                }
            }
            return hris.ToArray();
        }

        /// <summary>
        /// 获取后台任务队列中所有内置任务对象实例
        /// </summary>
        /// <param name="start"></param>
        /// <param name="tname"></param>
        /// <param name="maxStackSize"></param>
        /// <returns></returns>
        internal static KeyValuePair<BackgroundWorker, BGHandlerProxy>[] getActiveWorkers()
        {
            List<KeyValuePair<BackgroundWorker, BGHandlerProxy>> res = new List<KeyValuePair<BackgroundWorker, BGHandlerProxy>>();
            lock (BackendHandleMonitor_Lock)
            {
                int count = handleList.Count;
                int idx = 0;
                while (idx < count)
                {
                    HandleHolderI handle = handleList.ElementAt(idx);
                    if (handle != null && handle.isAlive() && handle is BackgroundWorkerHolder)
                    {
                        res.Add(handle.InnerUnit);
                    }
                    ++idx;
                }
            }
            return res.ToArray();
        }


        /// <summary>
        /// 查询句柄记录集，验证当前空闲状态
        /// </summary>
        /// <returns></returns>
        public static bool checkForIdle()
        {
            lock (BackendHandleMonitor_Lock)
            {
                int count = handleList.Count;
                int idx = 0;
                while (idx < count)
                {
                    HandleHolderI handle = handleList.ElementAt(idx);
                    if (handle == null || !handle.isAlive())
                    {
                        handleList.Remove(handle);
                    }
                    ++idx;
                }
            }
            return handleList.Count < 1;
        }

        /// <summary>
        /// 记录带参线程句柄对象
        /// </summary>
        /// <param name="start"></param>
        /// <param name="tname"></param>
        /// <param name="maxStackSize"></param>
        /// <returns></returns>
        public static Thread note(Thread thread)
        {
            if (thread != null && thread.IsAlive)
            {
                lock (BackendHandleMonitor_Lock)
                {
                    handleList.AddLast(new ThreadHolder(thread));
                }
            }
            return thread;
        }
        /// <summary>
        /// 记录异步线程句柄对象
        /// </summary>
        /// <returns></returns>
        public static BackgroundWorker note(BackgroundWorker worker, BGHandlerProxy proxy)
        {
            if (worker != null)
            {
                lock (BackendHandleMonitor_Lock)
                {
                    handleList.AddLast(new BackgroundWorkerHolder(worker, proxy));
                }
            }
            return worker;
        }

        /// <summary>
        /// 移除目标实例句柄对象
        /// </summary>
        /// <param name="hanlde"></param>
        internal static void removeHanlde(HandleHolderI hanlde)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(hanlde != null);
#endif
            lock (BackendHandleMonitor_Lock)
            {
                handleList.Remove(hanlde);
            }
        }
        /// <summary>
        /// 移除特定的后台任务实例句柄对象
        /// </summary>
        /// <param name="worker"></param>
        internal static void removeBackgroundworker(BackgroundWorker worker)
        {
            lock (BackendHandleMonitor_Lock)
            {
                int count = handleList.Count;
                int idx = 0;
                while (idx < count)
                {
                    HandleHolderI handle = handleList.ElementAt(idx);
                    if (handle != null && handle is BackgroundWorkerHolder)
                    {
                        if ((handle as BackgroundWorkerHolder).Contains(worker))
                        {
                            handleList.Remove(handle);
                            break;
                        }
                    }
                    ++idx;
                }
            }
        }
        #endregion

        #region Events&Handlings
        /// <summary>
        /// 句柄对象释放时事件处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected static void OnHandleDisposed(object sender, EventArgs e)
        {
            if (sender == null)
                return;
            if (sender is BackgroundWorker)
            {
                removeBackgroundworker(sender as BackgroundWorker);
            }
            if (sender is HandleHolderI)
            {
                removeHanlde(sender as HandleHolderI);
            }
        }
        #endregion

        #region Members
        /// <summary>
        /// 后台线程任务监控器独占保持的共享锁对象
        /// </summary>
        public static object BackendHandleMonitor_Lock = new object();
        /// <summary>
        /// 后台线程对象缓冲池
        /// @author JiahaiWu 2014-11-07
        /// </summary>
        private static LinkedList<HandleHolderI> handleList = new LinkedList<HandleHolderI>();
        #endregion

        #region 内置对象实现部分
        /// <summary>
        /// Nest class for BackgroundWorker Holder
        /// </summary>
        class BackgroundWorkerHolder : HandleHolderI
        {
            public BackgroundWorkerHolder(BackgroundWorker backWorker, BGHandlerProxy handlerProxy, string description = null)
            {
#if DEBUG
                System.Diagnostics.Debug.Assert(backWorker != null && handlerProxy != null);
#endif
                this.backWorker = backWorker;
                this.proxy = handlerProxy;
                this.desc = description;
                backWorker.Disposed += RunningHandlerNoter.OnHandleDisposed;
            }
            private readonly BackgroundWorker backWorker;
            private readonly BGHandlerProxy proxy;
            private readonly string desc;
            /// <summary>
            /// 转换生成线程对象的存活状态快照
            /// </summary>
            /// <returns></returns>
            public HandleRunInfo ToHandleRunInfo()
            {
                HandleRunStatus status = HandleRunStatus.Unknown;
                if (backWorker != null && backWorker.IsBusy)
                {
                    status = HandleRunStatus.InWorking;
                }
                if (proxy == null)
                    return null;
                HandleRunInfo hri = new HandleRunInfo(proxy.getProxyName(), status.ToString(), proxy.getRunningTime());
                hri.Description = desc;
                hri.handleType = typeof(BackgroundWorker).Name;
                return hri;
            }
            /// <summary>
            /// 获取线程对象存活与否状态
            /// </summary>
            /// <returns></returns>
            public bool isAlive()
            {
                return (backWorker != null && backWorker.IsBusy);
            }
            /// <summary>
            /// 获取内置句柄实例
            /// 说明：
            /// 1.return KeyValuePair<BackgroundWorker, LocalHandlerProxy>
            /// </summary>
            public dynamic InnerUnit
            {
                get
                {
                    return new KeyValuePair<BackgroundWorker, BGHandlerProxy>(backWorker, proxy);
                }
            }
            /// <summary>
            /// 判断是否存储同一实例的后台线程任务句柄
            /// </summary>
            /// <param name="worker"></param>
            /// <returns></returns>
            public bool Contains(BackgroundWorker worker)
            {
                if (backWorker != null && backWorker == worker)
                {
                    return true;
                }
                return false;
            }
        }


        /// <summary>
        /// Nest class for Thread Holder
        /// </summary>
        class ThreadHolder : HandleHolderI
        {
            public ThreadHolder(Thread thread, string description = null)
            {
#if DEBUG
                System.Diagnostics.Debug.Assert(thread != null);
                System.Diagnostics.Debug.Assert(!thread.IsBackground && thread.IsThreadPoolThread, "Not support thread type yet!");
#endif
                this.thread = thread;
                this.from = DateTime.Now;
                this.desc = description;
            }
            private readonly Thread thread;
            private readonly DateTime from;
            private readonly string desc;
            /// <summary>
            /// 转换生成线程对象的存活状态快照
            /// </summary>
            /// <returns></returns>
            public HandleRunInfo ToHandleRunInfo()
            {
                HandleRunStatus status = HandleRunStatus.Unknown;
                if (thread != null)
                {
                    if (thread.IsAlive)
                        status = HandleRunStatus.InWorking;
                    else
                    {
                        switch (thread.ThreadState)
                        {
                            case ThreadState.Running:
                            case ThreadState.Suspended:
                            case ThreadState.AbortRequested:
                            case ThreadState.WaitSleepJoin:
                            case ThreadState.SuspendRequested:
                                status = HandleRunStatus.InWorking;
                                break;
                            case ThreadState.Unstarted:
                                status = HandleRunStatus.Idle;
                                break;
                            case ThreadState.StopRequested:
                            case ThreadState.Stopped:
                                status = HandleRunStatus.Finished;
                                break;
                            case ThreadState.Aborted:
                            case ThreadState.Background:
                            default:
                                status = HandleRunStatus.Unknown;
                                break;
                        }
                    }
                }
                if (thread == null)
                    return null;
                TimeSpan span = DateTime.Now - from;
                HandleRunInfo hri = new HandleRunInfo(thread.Name, status.ToString(), span.Ticks);
                hri.Description = desc;
                hri.handleType = typeof(Thread).Name;
                return hri;
            }
            /// <summary>
            /// 获取线程对象存活与否状态
            /// </summary>
            /// <returns></returns>
            public bool isAlive()
            {
                return (thread != null && thread.IsAlive);
            }
            /// <summary>
            /// 获取内置句柄实例
            /// </summary>
            public dynamic InnerUnit
            {
                get
                {
                    return thread;
                }
            }
        }
        #endregion

    }
}
