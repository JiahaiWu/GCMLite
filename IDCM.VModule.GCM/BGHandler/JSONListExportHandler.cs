using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.Base.AbsInterfaces;
using IDCM.Core;
using IDCM.MsgDriver;
using IDCM.DataTransfer;


namespace IDCM.BGHandler
{
    class JSONListExportHandler : AbsBGHandler
    {
        public JSONListExportHandler(CTableCache ctableCache, string fpath)
        {
            this.xlsPath = System.IO.Path.GetFullPath(fpath);
            this.ctableCache = ctableCache;
        }

        public JSONListExportHandler(CTableCache ctableCache, string fpath, int[] selectedRows)
        {
            this.ctableCache = ctableCache;
            this.xlsPath = fpath;
            this.selectedRows = selectedRows;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override Object doWork(bool cancel, List<Object> args)
        {
            bool res = false;
            try
            {
                DCMPublisher.noteJobProgress(0);
                JSONListExporter exporter = new JSONListExporter();
                if (selectedRows != null)
                    res = exporter.exportJSONList(ctableCache, xlsPath, selectedRows);
                else
                    res = exporter.exportJSONList(ctableCache, xlsPath);
            }
            catch (Exception ex)
            {
                log.Error("JSON文件导出失败！ ", ex);
                DCMPublisher.noteSimpleMsg("ERROR: JSON文件导出失败！ " + ex.Message, IDCM.Base.ComPO.DCMMsgType.Alert);
            }
            return new object[] { res };
        }
        /// <summary>
        /// 后台任务执行结束，回调代码段
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override void complete(bool canceled, Exception error, List<Object> args)
        {
            DCMPublisher.noteJobProgress(100);
            DCMPublisher.noteJobFeedback(Base.ComPO.AsyncMsgNotice.LocalDataExported);
            if (canceled)
                return;
            if (error != null)
            {
                log.Error(error);
                log.Info("Export Failed! @filepath=" + xlsPath);
            }
            else
            {
                log.Info("Export success. @filepath=" + xlsPath);
            }
        }
        private string xlsPath = null;
        private CTableCache ctableCache = null;
        private int[] selectedRows;
    }
}
