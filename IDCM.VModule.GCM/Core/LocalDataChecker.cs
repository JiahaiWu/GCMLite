using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.ComponentUtil;
using System.Windows.Forms;
using System.Collections;

namespace IDCM.Core
{
    class LocalDataChecker
    {
        /// <summary>
        /// 检查可见表单数据的内容有效性
        /// </summary>
        /// <returns></returns>
        public static bool checkForExport(CTableCache ctcache, int maxErrorCount=1000)
        {
            Dictionary<int, CustomColValidator> checkers = new Dictionary<int, CustomColValidator>();
            foreach(KeyValuePair<string,int> pair in ctcache.getIAttrMapping())
            {
                checkers[pair.Value]=new CustomColValidator(CustomColDefGetter.getCustomColDef(pair.Key));
            }
            int ridx=0;
            int errorCount=0;
            while(ridx<ctcache.getRowCount())
            {
                lock(ctcache.GSyncRoot)
                {
                    DataGridViewRow dgvr = ctcache.getDGVRow(ridx);
                    errorCount+=checkForExport(dgvr,checkers);
                }
                if (errorCount > maxErrorCount)
                    break;
            }
            return errorCount <1;
        }
        /// <summary>
        /// 检查可见表单数据的内容有效性
        /// </summary>
        /// <returns></returns>
        public static bool checkForExport(CTableCache ctcache,ICollection selectedRows, int maxErrorCount = 1000)
        {
            Dictionary<int, CustomColValidator> checkers = new Dictionary<int, CustomColValidator>();
            foreach (KeyValuePair<string, int> pair in ctcache.getIAttrMapping())
            {
                checkers[pair.Value] = new CustomColValidator(CustomColDefGetter.getCustomColDef(pair.Key));
            }
            int errorCount = 0;
            foreach(object obj in selectedRows)
            {
                DataGridViewRow dgvr = (DataGridViewRow)obj; 
                if (!dgvr.IsNewRow)
                {
                    lock (ctcache.GSyncRoot)
                    {
                        errorCount += checkForExport(dgvr, checkers);
                    }
                    if (errorCount > maxErrorCount)
                        break;
                }
            }
            return errorCount < 1;
        }
        /// <summary>
        /// 检查可见表单数据的内容有效性
        /// </summary>
        /// <returns></returns>
        public static bool checkForExport(CTableCache ctcache,DataGridViewSelectedRowCollection selectedRows, int maxErrorCount = 1000)
        {
            Dictionary<int, CustomColValidator> checkers = new Dictionary<int, CustomColValidator>();
            foreach (KeyValuePair<string, int> pair in ctcache.getIAttrMapping())
            {
                checkers[pair.Value] = new CustomColValidator(CustomColDefGetter.getCustomColDef(pair.Key));
            }
            int errorCount = 0;
            foreach(DataGridViewRow dgvr in selectedRows)
            {
                if (dgvr.IsNewRow)
                    continue;
                lock (ctcache.GSyncRoot)
                {
                    errorCount += checkForExport(dgvr, checkers);
                }
                if (errorCount > maxErrorCount)
                    break;
            }
            return errorCount < 1;
        }


        public static int checkForExport(DataGridViewRow dgvr, Dictionary<int, CustomColValidator> checkers)
        {
            int errorCount = 0;

            foreach (KeyValuePair<int, CustomColValidator> chPair in checkers)
            {
                DataGridViewCell dgvc = dgvr.Cells[chPair.Key];
                string cellValue = DGVUtil.getCellValue(dgvc);
                bool isValid = chPair.Value.checkValid(cellValue);
                ControlAsyncUtil.SyncInvoke(dgvc.DataGridView, new ControlAsyncUtil.InvokeHandler(delegate()
                {
                    if (isValid)
                        dgvc.ErrorText = null;
                    else
                        dgvc.ErrorText = chPair.Value.getValidDescription();
                }));
                if (!isValid)
                    ++errorCount;
            }
            return errorCount;
        }
    }

    
}
