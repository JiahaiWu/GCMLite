using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using IDCM.Forms;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using IDCM.Base;


namespace IDCM.Core
{
    class DataExportChecker
    {
        #region Methods
       
        /// <summary>
        /// 解析指定的GCM Pub XML文档，验证数据转换的属性映射条件.
        /// </summary>
        /// <param name="fpath"></param>
        /// <returns></returns>
        internal static bool checkForGCMPubXMLExport(ref Dictionary<string, string> dataMapping)
        {
            List<string> gcmCols = fetchPublishGCMFields();
            if (gcmCols == null || gcmCols.Count < 1)
                throw new IDCMException("Load mapping attrs from GCM Pub XML template failed.");
            ICollection<string> viewCols = Core.CustomColDefGetter.getCustomCols();
            ///////////////////////////////////////////////////////////////
            using (AttrMapOptionDlg amoDlg = new AttrMapOptionDlg())
            {
                amoDlg.BringToFront();
                amoDlg.setInitCols(viewCols, gcmCols, ref dataMapping);
                amoDlg.ShowDialog();
                ///////////////////////////////////////////
                if (amoDlg.DialogResult == DialogResult.OK)
                    return true;
            }
            return false;
        }

        private static List<string> fetchPublishGCMFields()
        {
            Dictionary<string, int> gcmCols = new Dictionary<string, int>();
            string fpath = ConfigurationManager.AppSettings.Get(SysConstants.GCMUploadTemplate);
            if (fpath == null || fpath.Length < 1)
                return null;
            XmlDocument xmlDoc = new XmlDocument();
            using (FileStream fs = new FileStream(fpath, FileMode.Open, FileAccess.Read))
            {
                xmlDoc.Load(fs);
                XmlNode sxnode = xmlDoc.SelectSingleNode("/strains/strain");
                if (sxnode == null)
                    return null;
                foreach (XmlNode attrNode in sxnode.ChildNodes)
                {
                    gcmCols[attrNode.Name] = 0;
                }
            }
            return gcmCols.Keys.ToList();
        }
        #endregion
    }
}
