using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Core;
using IDCM.MsgDriver;
using IDCM.BGHandlerManager;
using IDCM.DataTransfer;
using IDCM.Base.ComPO;

namespace IDCM.BGHandler
{
    class TextImportHandler: AbsBGHandler
    {
        public TextImportHandler(CTableCache ctcache, string fpath, ExportType etype, ref Dictionary<string, string> dataMapping)
        {
            this.ctcache = ctcache;
            this.txtPath = System.IO.Path.GetFullPath(fpath);
            this.dataMapping = dataMapping;
            this.txtType = etype;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override object doWork(bool cancel, List<object> args)
        {
            bool res = false;
            try
            {
                DCMPublisher.noteJobProgress(0);
                switch (txtType)
                {
                    case ExportType.CSV:
                        res = TextDataImporter.parseCSVData(ctcache, txtPath, ref dataMapping);
                        break;
                    case ExportType.TSV:
                        res = TextDataImporter.parseTSVData(ctcache, txtPath, ref dataMapping);
                        break;
                    default:
                        res = false;
                        break;
                }
                if (res)
                {
                    DCMPublisher.noteJobFeedback(AsyncMsgNotice.LocalDataImported);
                }
            }
            catch (Exception ex)
            {
                log.Error(IDCM.Base.GlobalTextRes.Text("Failed to import text file") + "！", ex);
                DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to import text file") + "！ " + ex.Message, DCMMsgType.Alert);
            }
            return new object[] { res, txtPath };
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
                DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to import text file") + "！ " + error.Message, DCMMsgType.Alert);
                return;
            }
        }
        public override void addHandler(AbsBGHandler nextHandler)
        {
            base.addHandler(nextHandler);
        }
        private CTableCache ctcache;
        private ExportType txtType;
        private string txtPath = null;
        private Dictionary<string, string> dataMapping = null;
    }
}
