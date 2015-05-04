using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCM.Base.AbsInterfaces
{
    public interface IBGHandler
    {
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        Object doWork(bool cancel, List<Object> args);
        /// <summary>
        /// 后台任务执行结束，回调代码段
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        void complete(bool canceled, Exception error, List<Object> args);

        /// <summary>
        /// 后台任务执行过程中的状态反馈代码段。
        /// 如dowork的执行部分无状态反馈过程，本方法仅需空实现即可。
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="progressPercentage"></param>
        /// <param name="args"></param>
        void progressChanged(int progressPercentage, List<Object> args);
    }
}
