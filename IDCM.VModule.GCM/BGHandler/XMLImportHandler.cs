﻿using IDCM.BGHandlerManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.Core;
using IDCM.DataTransfer;
using IDCM.MsgDriver;
using System.Data;

namespace IDCM.BGHandler
{
    public class XMLImportHandler : AbsBGHandler
    {
        public XMLImportHandler(CTableCache ctcache, string fpath, ref Dictionary<string, string> dataMapping)
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
        public override object doWork(bool cancel, List<object> args)
        {
            bool res = false;
            try
            {
                DCMPublisher.noteJobProgress(0);
                res = XMLDataImporter.parseXMLData(ctcache, xlsPath, ref dataMapping);
                if (res)
                {
                    DCMPublisher.noteJobFeedback(AsyncMsgNotice.LocalDataImported);
                }
            }
            catch (Exception ex)
            {
                log.Error(IDCM.Base.GlobalTextRes.Text("Failed to import XML document")+"！", ex);
                DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to import XML document") + "！" + ex.Message, DCMMsgType.Alert);
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
            if (canceled)
                return;
            if (error != null)
            {
                log.Error(error);
                DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to import XML document") + "！" + error.Message, DCMMsgType.Alert);

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
