using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using IDCM.Core;
using System.Xml;
using IDCM.Base;

namespace IDCM.DataTransfer
{
    class XMLExporter
    {
        /// <summary>
        /// 根据历史查询条件导出目标文本数据集
        /// <returns></returns>
        public bool exportXML(CTableCache ctcache, string filepath)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                int tcount = ctcache.getRowCount();
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    strbuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
                    strbuilder.Append("<strains>\r\n");
                    Dictionary<string, int> maps = ctcache.getIAttrMapping();
                    ///////////////////
                    int ridx = 0;
                    while (ridx < tcount)
                    {
                        Dictionary<int, string> drow = ctcache.getIRow(ridx);
                        if (drow != null)
                        {
                            XmlElement xmlEle = convertToXML(xmlDoc, maps, drow);
                            strbuilder.Append(xmlEle.OuterXml).Append("\r\n");
                            /////////////
                            if (++ridx % 100 == 0)
                            {
                                Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                                BinaryWriter bw = new BinaryWriter(fs);
                                fs.Write(info, 0, info.Length);
                                strbuilder.Length = 0;
                            }
                        }
                    }
                    strbuilder.Append("</strains>");
                    if (strbuilder.Length > 0)
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                        BinaryWriter bw = new BinaryWriter(fs);
                        fs.Write(info, 0, info.Length);
                        strbuilder.Length = 0;
                    }
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new IDCMException("Error raised in exportXML(...)", ex);
            }
            return true;
        }
        /// <summary>
        /// 以Excle导出数据，数据源DataGridViewSelectedRowCollection
        /// </summary>
        /// <param name="filepath">导出路径</param>
        /// <param name="selectedRows">数据源</param>
        /// <returns></returns>
        internal bool exportXML(CTableCache ctcache, string filepath, int[] selectedRows)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                int count = 0;
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    strbuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
                    strbuilder.Append("<strains>\r\n");
                    Dictionary<string, int> maps = ctcache.getIAttrMapping();
                    ///////////////////
                    for (int ridx = selectedRows.Length - 1; ridx >= 0; ridx--)
                    {
                        int lindex = selectedRows[ridx];
                        Dictionary<int, string> drow = ctcache.getIRow(lindex);
                        if (drow != null)
                        {
                            XmlElement xmlEle = convertToXML(xmlDoc, maps, drow);
                            strbuilder.Append(xmlEle.OuterXml).Append("\r\n");
                        }
                        if (++count % 100 == 0)
                        {
                            Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                            BinaryWriter bw = new BinaryWriter(fs);
                            fs.Write(info, 0, info.Length);
                            strbuilder.Length = 0;
                        }
                    }
                    strbuilder.Append("</strains>");
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                        BinaryWriter bw = new BinaryWriter(fs);
                        fs.Write(info, 0, info.Length);
                        strbuilder.Length = 0;
                    }
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new IDCMException("Error raised in exportXML(...)", ex);
            }
            return true;
        }
        /// <summary>
        /// 根据字段将一行记录转换成xmlElement
        private static XmlElement convertToXML(XmlDocument xmlDoc, Dictionary<string, int> maps, Dictionary<int,string> row)
        {
            XmlElement strainEle = xmlDoc.CreateElement("strain");
            foreach (KeyValuePair<string, int> mapEntry in maps)
            {
                XmlElement attrEle = xmlDoc.CreateElement(mapEntry.Key);
                attrEle.InnerText = row[mapEntry.Value];
                strainEle.AppendChild(attrEle);
            }
            return strainEle;
        }
        /// <summary>
        /// 根据字段将一行记录转换成xmlElement
        private static XmlElement convertToXML(XmlDocument xmlDoc, Dictionary<string, string> maps, Dictionary<string, string> row)
        {
            XmlElement strainEle = xmlDoc.CreateElement("strain");
            foreach (KeyValuePair<string, string> mapEntry in maps)
            {
                XmlElement attrEle = xmlDoc.CreateElement(mapEntry.Key);
                attrEle.InnerText = row[mapEntry.Value];
                strainEle.AppendChild(attrEle);
            }
            return strainEle;
        }
        internal bool exportGCMXML(CTableCache ctcache, MemoryStream xmlStream, Dictionary<string, string> dataMapping)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                int tcount = ctcache.getRowCount();
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    strbuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
                    strbuilder.Append("<strains>\r\n");
                    ///////////////////
                    int ridx = 0;
                    while (ridx < tcount)
                    {
                        Dictionary<string, string> drow = ctcache.getRow(ridx);
                        if (drow != null)
                        {
                            XmlElement xmlEle = convertToXML(xmlDoc, dataMapping, drow);
                            strbuilder.Append(xmlEle.OuterXml).Append("\r\n");
                            /////////////
                            if (++ridx % 100 == 0)
                            {
                                Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                                xmlStream.Write(info, 0, info.Length);
                                strbuilder.Length = 0;
                            }
                        }
                    }
                    strbuilder.Append("</strains>");
                    if (strbuilder.Length > 0)
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                        xmlStream.Write(info, 0, info.Length);
                        strbuilder.Length = 0;
                    }
                    xmlStream.Position = 0;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new IDCMException("Error raised in exportGCMXML(...)", ex);
            }
            return true;
        }
        internal bool exportGCMXML(CTableCache ctcache, MemoryStream xmlStream, Dictionary<string, string> dataMapping, int[] selectedRows)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                int count = 0;
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    strbuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
                    strbuilder.Append("<strains>\r\n");
                    ///////////////////
                    for (int ridx = selectedRows.Length - 1; ridx >= 0; ridx--)
                    {
                        int lindex = selectedRows[ridx];
                        Dictionary<string, string> drow = ctcache.getRow(lindex);
                        if (drow != null)
                        {
                            XmlElement xmlEle = convertToXML(xmlDoc, dataMapping, drow);
                            strbuilder.Append(xmlEle.OuterXml).Append("\r\n");
                        }
                        if (++count % 100 == 0)
                        {
                            Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                            xmlStream.Write(info, 0, info.Length);
                            strbuilder.Length = 0;
                        }
                    }
                    strbuilder.Append("</strains>");
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                        xmlStream.Write(info, 0, info.Length);
                        strbuilder.Length = 0;
                    }
                    xmlStream.Position = 0;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new IDCMException("Error raised in exportGCMXML(...)", ex);
            }
            return true;
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();  
    }
}
