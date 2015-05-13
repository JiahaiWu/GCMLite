using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.Base;
using System.IO;
using System.Configuration;
using IDCM.Base.Utils;
using System.Xml;

namespace IDCM.Core
{
    public class CustomColDefGetter
    {
        public static ICollection<CustomColDef> getCustomTableDef()
        {
            if (ccdCache != null)
                return ccdCache.Values;
            List<CustomColDef> ccds = null;
            try
            {
                string srcHashCode = null;
                string cacheDir = SysConstants.initEnvDir + SysConstants.cacheDir;
                if (!File.Exists(cacheDir))
                    Directory.CreateDirectory(cacheDir);
                string hisCfg = cacheDir + SysConstants.tableDefNote;
                string userCfg = Path.GetDirectoryName(SysConstants.exePath) + Path.DirectorySeparatorChar + ConfigurationManager.AppSettings[SysConstants.CTableDef];
                if (File.Exists(userCfg))
                {
                    FileStream fs = new FileStream(userCfg, FileMode.Open);
                    srcHashCode = HashUtil.md5HexCode(fs);
                    fs.Close();
                }
                if (File.Exists(hisCfg))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    using (FileStream xfs = new FileStream(hisCfg, FileMode.Open, FileAccess.Read))
                    {
                        xmlDoc.Load(xfs);
                        XmlNode lastSrcNode = xmlDoc.SelectSingleNode("/CTableConfig/lastHashCode");
                        XmlNode sxnode = xmlDoc.SelectSingleNode("/CTableConfig/fields");
                        lastSrcHashCode = lastSrcNode != null && lastSrcNode.InnerText.Length > 0 ? lastSrcNode.InnerText.Trim() : null;
                        if (sxnode!=null && srcHashCode == null || srcHashCode.Equals(lastSrcHashCode))
                        {
                            ccds = CustomColDef.getCustomTableDef(sxnode);
                            setKeyColDef(xmlDoc,ccds);
                        }
                    }
                }
                if(ccds==null)
                {
#if DEBUG
                    if (!userCfg.Contains("IDE"))
                    {
                        if (!File.Exists(userCfg))
                            throw new IDCMException("缺少数据表配置文件！ @path=" + userCfg);
                        ccds = CustomColDef.getCustomTableDef(userCfg);
                        setKeyColDef(userCfg,ccds);
                        lastSrcHashCode = srcHashCode;
                    }
#else
                    if (!File.Exists(userCfg))
                        throw new IDCMException("缺少数据表配置文件！ @path="+ userCfg);
                    ccds = CustomColDef.getCustomTableDef(userCfg);
                    lastSrcHashCode = srcHashCode;
#endif
                }
            }
            catch (IOException ex)
            {
                throw new IDCMException("数据表配置文件读取异常", ex);
            }
            if (ccds != null)
            {
                ccdCache = new Dictionary<string, CustomColDef>();
                foreach (CustomColDef ccd in ccds.OrderBy(rs => rs.Corder))
                {
                    ccdCache.Add(ccd.Attr, ccd);
                }
            }
            return ccdCache!=null?ccdCache.Values:null;
        }

        private static void setKeyColDef(XmlDocument xmlDoc, List<CustomColDef> ccds)
        {
            primaryKeyNode = ccds.First();
            XmlNode keynode = xmlDoc.SelectSingleNode("/CTableConfig/keyFiled");
            if (keynode != null)
            {
                foreach (CustomColDef ccd in ccds)
                {
                    if(ccd.Attr.Equals(keynode.InnerText))
                    {
                        primaryKeyNode = ccd;
                        break;
                    }
                }
            }
        }

        private static void setKeyColDef(string userCfg, List<CustomColDef> ccds)
        {
            XmlDocument xmlDoc = new XmlDocument();
            using (FileStream xfs = new FileStream(userCfg, FileMode.Open, FileAccess.Read))
            {
                xmlDoc.Load(xfs);
                setKeyColDef(xmlDoc, ccds);
            }
        }

        public static bool saveUpdatedHistCfg()
        {
            if(dirtyStatus)
            {
                dirtyStatus=false;
                string cacheDir = SysConstants.initEnvDir + SysConstants.cacheDir;
                if (!File.Exists(cacheDir))
                    Directory.CreateDirectory(cacheDir);
                string hisCfg = cacheDir + SysConstants.tableDefNote;
                StringBuilder sb = new StringBuilder();
                XmlDocument xmlDoc = new XmlDocument();
                sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                sb.AppendLine("<CTableConfig>");
                XmlElement lastHashCode = xmlDoc.CreateElement("lastHashCode");
                XmlElement keyFiled = xmlDoc.CreateElement("keyFiled");
                XmlElement fields = xmlDoc.CreateElement("fields");
                lastHashCode.InnerText = lastSrcHashCode;
                keyFiled.InnerText = primaryKeyNode.Attr;
                foreach(CustomColDef ccd in ccdCache.Values)
                {
                    fields.AppendChild(ccd.toXmlElement(xmlDoc));
                }
                sb.AppendLine(lastHashCode.OuterXml);
                sb.AppendLine(keyFiled.OuterXml);
                sb.AppendLine(fields.OuterXml);
                sb.AppendLine("</CTableConfig>");
                return FileUtil.writeToUTF8File(hisCfg, sb.ToString());
            }
            return false;
        }
        public static ICollection<String> getCustomCols()
        {
            List<string> attrs = new List<string>();
            ICollection<CustomColDef> ccds = getCustomTableDef();
            if (ccds != null)
            {
                foreach (CustomColDef ccd in ccds)
                {
                    attrs.Add(ccd.Attr);
                }
            }
            return attrs;
        }
        public static CustomColDef getCustomColDef(string attr)
        {
            CustomColDef coldef = null;
            ccdCache.TryGetValue(attr, out coldef);
            return coldef;
        }
        internal static void updateCustomColDef(string attr, bool isEnable)
        {
            CustomColDef ccd = null;
            if (ccdCache.TryGetValue(attr,out ccd))
            {
                if (ccd.IsEnable != isEnable)
                {
                    ccd.IsEnable = isEnable;
                    dirtyStatus = true;
                }
            }
        }
        public static void updateCustomColCond(string attr, bool isRequire, bool isUnique, string restrict)
        {
            CustomColDef ccd = null;
            if (ccdCache.TryGetValue(attr, out ccd))
            {
                if (ccd.IsRequire != isRequire)
                {
                    ccd.IsRequire = isRequire;
                    dirtyStatus = true;
                }
                if (ccd.IsUnique != isUnique)
                {
                    ccd.IsUnique = isUnique;
                    dirtyStatus = true;
                }
                if (ccd.Restrict==null|| !ccd.Restrict.Equals(restrict))
                {
                    ccd.Restrict = restrict;
                    dirtyStatus = true;
                }
                saveUpdatedHistCfg();
            }
        }
        public static string KeyName
        {
            get
            {
                return primaryKeyNode.Attr;
            }
        }
        private volatile static bool dirtyStatus = false;
        private static string lastSrcHashCode=null;
        private static CustomColDef primaryKeyNode = null;
        private static Dictionary<string,CustomColDef> ccdCache = null;

        internal static void updateCustomColRestrict(string p, bool isRequire, bool isUnique, string restrict)
        {
            throw new NotImplementedException();
        }
    }
}
