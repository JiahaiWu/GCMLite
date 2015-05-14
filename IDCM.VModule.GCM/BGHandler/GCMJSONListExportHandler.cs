﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Core;
using System.IO;
using IDCM.Base;
using Newtonsoft.Json;
using IDCM.Base.AbsInterfaces;
using IDCM.DataTransfer;
using IDCM.MsgDriver;

namespace IDCM.BGHandler
{
    class GCMJSONListExportHandler : AbsBGHandler
    {
        public GCMJSONListExportHandler(GCMTableCache ctableCache, string fpath)
        {
            this.xlsPath = System.IO.Path.GetFullPath(fpath);
            this.ctableCache = ctableCache;
        }

        public GCMJSONListExportHandler(GCMTableCache ctableCache, string fpath, int[] selectedRows)
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
                GCMJSONListExporter exporter = new GCMJSONListExporter();
                if (selectedRows != null)
                    res = exporter.exportJSONList(ctableCache, xlsPath, selectedRows);
                else
                    res = exporter.exportJSONList(ctableCache, xlsPath);
            }
            catch (Exception ex)
            {
                log.Error(IDCM.Base.GlobalTextRes.Text("Failed to export JSON File")+"！ ", ex);
                DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to export JSON File") + "！ " + ex.Message, IDCM.Base.ComPO.DCMMsgType.Alert);
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
                log.Info(IDCM.Base.GlobalTextRes.Text("Export failed")+"! @filepath=" + xlsPath);
            }
            else
            {
                log.Info(IDCM.Base.GlobalTextRes.Text("Export success") + ". @filepath=" + xlsPath);
            }
        }
        private string xlsPath = null;
        private GCMTableCache ctableCache = null;
        private int[] selectedRows;
    }
}