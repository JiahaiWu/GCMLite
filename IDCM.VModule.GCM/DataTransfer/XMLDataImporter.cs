using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using IDCM.Core;
using System.Data;
using System.Threading;

namespace IDCM.DataTransfer
{
    class XMLDataImporter
    {
        #region Methods

        /// <summary>
        /// 解析指定的Excel文档，执行数据转换.
        /// 本方法调用对类功能予以线程包装，用于异步调用如何方法。
        /// 在本线程调用下的控件调用，需通过UI控件的Invoke/BegainInvoke方法更新。
        /// </summary>
        /// <param name="fpath"></param>
        /// <returns>返回请求流程是否执行完成</returns>
        public static bool parseXMLData(CTableCache ctcache, string fpath, ref Dictionary<string, string> dataMapping)
        {
            if (fpath == null || fpath.Length < 1)
                return false;
            string fullPaht = System.IO.Path.GetFullPath(fpath);
            XmlDocument xDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            using (XmlReader xRead = XmlReader.Create(fullPaht))
            {
                xDoc.Load(xRead);
                parseXMLMappingInfo(ctcache, xDoc, ref dataMapping);
                return true;
            }
        }
        public static void parseXMLMappingInfo(CTableCache ctcache, XmlDocument xDoc, ref Dictionary<string, string> dataMapping)
        {
            XmlNodeList strainChildNodes = xDoc.DocumentElement.ChildNodes;
            XmlNode strainNode = strainChildNodes[0];//获取第一个strainNode
            /////////////////////////////////////////////////////////////////////////////
            if (dataMapping != null && dataMapping.Count > 0)
            {
                while (strainNode != null)
                {
                    lock (ctcache.GSyncRoot)
                    {
                        Dictionary<string, string> mapvalues = new Dictionary<string, string>();

                        foreach (XmlNode attrNode in strainNode.ChildNodes)//循环的是strain -> strainAttr
                        {
                            string xmlAttrName = attrNode.Name;
                            string dbName = dataMapping[xmlAttrName];
                            string xmlAttrValue = attrNode.InnerText;
                            if (dbName != null && xmlAttrValue != null && xmlAttrValue.Length > 0)
                                mapvalues[dbName] = xmlAttrValue;
                        }
                        ctcache.addRow(mapvalues);
                    }
                    strainNode = strainNode.NextSibling;
                }
            }
        }
        #endregion

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
