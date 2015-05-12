using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.Base.AbsInterfaces;
using IDCM.MsgDriver;
using IDCM.Core;
using IDCM.DataTransfer;
using System.IO;
using IDCM.Base;

namespace IDCM.BGHandler
{
    class LocalDataPubHandler:AbsBGHandler
    {
        private Core.CTableCache ctcache;
        private ICollection selectedRows;

        public LocalDataPubHandler(Core.CTableCache ctcache)
        {
            // TODO: Complete member initialization
            this.ctcache = ctcache;
        }

        public LocalDataPubHandler(Core.CTableCache ctcache,DataGridViewRow[] selectedRows)
        {
            // TODO: Complete member initialization
            this.ctcache = ctcache;
            this.selectedRows = selectedRows;
        }

        public LocalDataPubHandler(Core.CTableCache ctcache,DataGridViewSelectedRowCollection selectedRows)
        {
            // TODO: Complete member initialization
            this.ctcache = ctcache;
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
                ///////////////////////////////////////////////////////////////////////////////////
                //Checking
                if (selectedRows != null)
                    res = LocalDataChecker.checkForExport(ctcache, selectedRows);
                else
                    res = LocalDataChecker.checkForExport(ctcache);
                if(!res)
                    return new object[] { res };
                ///////////////////////////////////////////////////////////////////////////////////
                //mapping & export
                string xmldata = null;
                Dictionary<string, string> dataMapping = new Dictionary<string, string>();
                if (DataExportChecker.checkForGCMPubXMLExport(ref dataMapping))
                {
                    XMLExporter exporter = new XMLExporter();
                    using (MemoryStream xmlStream = new MemoryStream())
                    {
                        if (selectedRows != null)
                        {
                            int[] ridxs = new int[selectedRows.Count];
                            int cc=0;
                            foreach (object obj in selectedRows)
                            {
                                DataGridViewRow dgvr = (DataGridViewRow)obj;
                                ridxs[cc] =dgvr.Index;
                                cc++;
                            }
                            res = exporter.exportGCMXML(ctcache, xmlStream, dataMapping, ridxs);
                        }
                        else
                            res = exporter.exportGCMXML(ctcache, xmlStream, dataMapping);
                        if (res)
                        {
                            xmldata = SysConstants.defaultEncoding.GetString(xmlStream.ToArray());
                        }
                    }
                }
                ////////////////////////////////////////////////////////////////////////////////////
                //validation

                ////////////////////////////////////////////////////////////////////////////////////
                //publish
                
                ////////////////////////////////////////////////////////////////////////////////////
            }
            catch (Exception ex)
            {
                log.Error("XML文件导出失败！ ", ex);
                DCMPublisher.noteSimpleMsg("ERROR: XML文件导出失败！ " + ex.Message, IDCM.Base.ComPO.DCMMsgType.Alert);
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
            //DCMPublisher.noteJobFeedback(Base.ComPO.AsyncMsgNotice.LocalDataChecked);
            //if (canceled)
            //    return;
            //if (error != null)
            //{
            //    log.Error(error);
            //    log.Info("Data Check Failed!");
            //}
            //else
            //{
            //    log.Info("Data Check success.");
            //}
        }
    }
}
