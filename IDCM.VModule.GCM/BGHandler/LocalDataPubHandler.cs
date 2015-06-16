using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.MsgDriver;
using IDCM.Core;
using IDCM.DataTransfer;
using System.IO;
using IDCM.Base;
using IDCM.ComPO;
using IDCM.BGHandlerManager;

namespace IDCM.BGHandler
{
    class LocalDataPubHandler:AbsBGHandler
    {
        private Core.CTableCache ctcache;
        private ICollection selectedRows;
        private AuthInfo authInfo;

        public LocalDataPubHandler(AuthInfo authInfo,Core.CTableCache ctcache)
        {
            this.authInfo=authInfo;
            this.ctcache = ctcache;
        }

        public LocalDataPubHandler(AuthInfo authInfo,Core.CTableCache ctcache,DataGridViewRow[] selectedRows)
        {
            this.authInfo=authInfo;
            this.ctcache = ctcache;
            this.selectedRows = selectedRows;
        }

        public LocalDataPubHandler(AuthInfo authInfo,Core.CTableCache ctcache,DataGridViewSelectedRowCollection selectedRows)
        {
            this.authInfo=authInfo;
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
            string errorInfo=null;
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
                {
                    errorInfo=GlobalTextRes.Text("Local data check failed.");
                    DCMPublisher.noteSimpleMsg(errorInfo);
                    return new object[] { res,errorInfo };
                }
                ///////////////////////////////////////////////////////////////////////////////////
                //mapping
                Dictionary<string, string> dataMapping = new Dictionary<string, string>();
                if (DataExportChecker.checkForGCMPubXMLExport(ref dataMapping))
                {
                    ////////////////////////////////////////////////////////////////////////////////////////
                    //export
                    XMLExporter exporter = new XMLExporter();
                    using (MemoryStream xmlStream = new MemoryStream())
                    {
                        if (selectedRows != null)
                        {
                            int[] ridxs = new int[selectedRows.Count];
                            int cc = 0;
                            foreach (object obj in selectedRows)
                            {
                                DataGridViewRow dgvr = (DataGridViewRow)obj;
                                ridxs[cc] = dgvr.Index;
                                cc++;
                            }
                            res = exporter.exportGCMXML(ctcache, xmlStream, dataMapping, ridxs);
                        }
                        else
                            res = exporter.exportGCMXML(ctcache, xmlStream, dataMapping);
                        if (!res)
                        {
                            errorInfo = GlobalTextRes.Text("Local data export to GCMPub XML failed.");
                            DCMPublisher.noteSimpleMsg(errorInfo);
                            return new object[] { res, errorInfo };
                        }
                        ////////////////////////////////////////////////////////////////////////////////////
                        //validation
                        if (GCMDataChecker.checkForPublish(xmlStream, out errorInfo))
                        {
                            ////////////////////////////////////////////////////////////////////////////////////
                            //publish
                            XMLImportStrainsRes importRes = GCMPubExecutor.xmlImportStrains(xmlStream,authInfo);
                            if (importRes.msg_num.Equals("2"))
                            {
                                res = true;
                                //提交成功
                                DCMPublisher.noteSimpleMsg(GlobalTextRes.Text("Local data publish to GCM operation success."));
                            }
                            else
                            {
                                res = false;
                                errorInfo = GlobalTextRes.Text("Exported XML Validation Failed")+": " + errorInfo;
                                DCMPublisher.noteSimpleMsg(errorInfo);
                                return new object[] { res, errorInfo };
                            }
                            ////////////////////////////////////////////////////////////////////////////////////
                        }
                        else
                        {
                            res = false;
                            errorInfo = GlobalTextRes.Text("Exported XML Validation Failed")+": "+errorInfo;
                            DCMPublisher.noteSimpleMsg(errorInfo);
                            return new object[] { res, errorInfo };
                        }
                    }
                }
                else
                {
                    res = false;
                    errorInfo = GlobalTextRes.Text("Local data mapping to export canceled.");
                    DCMPublisher.noteSimpleMsg(errorInfo);
                    return new object[] { res, errorInfo };
                }
            }
            catch (Exception ex)
            {
                log.Error(IDCM.Base.GlobalTextRes.Text("Failed to export and publish the XML document for GCM") + "！ ", ex);
                DCMPublisher.noteSimpleMsg("ERROR: "+IDCM.Base.GlobalTextRes.Text("Failed to export and publish the XML document for GCM") + "！ " + ex.Message, DCMMsgType.Alert);
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
            
            if (canceled)
                return;
            if (error != null)
            {
                log.Error(error);
                log.Info(IDCM.Base.GlobalTextRes.Text("Data publish failed")+"!");
                DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to export and publish the XML document for GCM") + "！ " + error.Message, DCMMsgType.Alert);
            }
            else
            {
                if (args.Count > 1)
                {
                    bool res = (bool)args[0];
                    string errorInfo = args[1].ToString();
                    if (!res)
                    {
                        log.Error(errorInfo);
                        log.Info(IDCM.Base.GlobalTextRes.Text("Data publish failed") + "!");
                        DCMPublisher.noteSimpleMsg("ERROR: " + IDCM.Base.GlobalTextRes.Text("Failed to export and publish the XML document for GCM") + "！ " + errorInfo, DCMMsgType.Alert);
                        return;
                    }
                }
                log.Info(IDCM.Base.GlobalTextRes.Text("Data publish success") + ".");
                DCMPublisher.noteJobFeedback(AsyncMsgNotice.LocalDataPublished);
            }
        }
    }
}
