using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.Base.AbsInterfaces;
using IDCM.Core;
using IDCM.MsgDriver;
using System.Windows.Forms;
using System.ComponentModel;
using IDCM.DataTransfer;
using System.Data;

namespace IDCM.BGHandler
{
    class ExcelExportHandler:AbsBGHandler
    {
        public ExcelExportHandler(CTableCache ctcache, string xlsPath)
        {
            this.ctcache = ctcache;
            this.xlsPath = System.IO.Path.GetFullPath(xlsPath);
        }
        public ExcelExportHandler(CTableCache ctcache, string xlsPath, int[] selectedRows)
        {
            this.ctcache = ctcache;
            this.xlsPath = System.IO.Path.GetFullPath(xlsPath);
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
            try{
                DCMPublisher.noteJobProgress(0);
                ExcelExporter exporter = new ExcelExporter();
                if (selectedRows != null)
                    res = exporter.exportExcel(ctcache, xlsPath, selectedRows);
                else
                    res = exporter.exportExcel(ctcache, xlsPath);
            }
            catch (Exception ex)
            {
                log.Error("XML文件导入失败！ ", ex);
                DCMPublisher.noteSimpleMsg("ERROR: XML文件导入失败！ " + ex.Message, IDCM.Base.ComPO.DCMMsgType.Alert);
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
        private int[] selectedRows;
        private CTableCache ctcache;
    }
}
