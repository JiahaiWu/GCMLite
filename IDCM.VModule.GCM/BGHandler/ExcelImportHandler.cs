using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.MsgDriver;
using System.Windows.Forms;
using IDCM.DataTransfer;
using System.Data;
using IDCM.Core;
using IDCM.BGHandlerManager;

namespace IDCM.BGHandler
{
    /// <summary>
    /// 将目标excel文档导入至目标数据库
    /// </summary>
    public class ExcelImportHandler : AbsBGHandler
    {
        public ExcelImportHandler(CTableCache ctcache, string fpath, ref Dictionary<string, string> dataMapping)
        {
            this.xlsPath = System.IO.Path.GetFullPath(fpath);
            this.dataMapping = dataMapping;
            this.ctcache = ctcache;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override Object doWork(bool cancel, List<Object> args)
        {
            bool res = false;
            try{
                DCMPublisher.noteJobProgress(0);
                res = ExcelDataImporter.parseExcelData(ctcache, xlsPath, ref dataMapping);
            }
            catch (Exception ex)
            {
                log.Info("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to Export excel file") + "！ ", ex);
                DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to Export excel file") + "！ " + ex.Message, DCMMsgType.Alert);
            }
            return new object[] { res, xlsPath };
        }
        /// <summary>
        /// 后台任务执行结束，回调代码段
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override void complete(bool canceled, Exception error, List<Object> args)
        {
            DCMPublisher.noteJobProgress(100);
            DCMPublisher.noteJobFeedback(AsyncMsgNotice.LocalDataImported);
            if (canceled)
                return;
            if (error != null)
            {
                log.Error(error);
                return;
            }
        }
        public override void addHandler(AbsBGHandler nextHandler)
        {
            base.addHandler(nextHandler);
        }
        private string xlsPath = null;
        private Dictionary<string, string> dataMapping = null;
        private CTableCache ctcache;
    }
}
