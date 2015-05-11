using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Core
{
    class DataExportChecker
    {
        /// <summary>
        /// 解析指定的XML文档，验证数据转换的属性映射条件.
        /// </summary>
        /// <param name="fpath"></param>
        /// <returns></returns>
        internal static bool checkForXMLExport(string fpath, ref Dictionary<string, string> dataMapping)
        {
            //if (fpath == null || fpath.Length < 1)
            //    return false;
            //string fullPath = System.IO.Path.GetFullPath(fpath);
            //try
            //{
            //    XmlDocument xDoc = new XmlDocument();
            //    XmlReaderSettings settings = new XmlReaderSettings();
            //    settings.IgnoreComments = true;
            //    using (XmlReader xRead = XmlReader.Create(fullPath))
            //    {
            //        xDoc.Load(xRead);
            //        return fetchXMLMappingInfo(xDoc, ref dataMapping) && dataMapping.Count > 0;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error("ERROR: XML文件导入失败！ ", ex);
            //    DCMPublisher.noteSimpleMsg("ERROR: XML文件导入失败！ " + ex.Message + "\n" + ex.ToString());
            //}
            return false;
        }
    }
}
