using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IDCM.Base.Utils;
using IDCM.Base;
using System.Xml;

namespace IDCM.Core
{
    /// <summary>
    /// IDCM Custom Table Definition
    /// @Date 2015-04-20
    /// @author JiahaiWu 
    /// @Description:
    /// 字段名称	必须非空	必须唯一	校验表达式	别名	默认值
    /// 对应于
    /// attr	Require	Unique	Restrict	Alias	DefaultVal
    /// attr 字段名称不支持制表符、换行符，由fields节点下的直接孩子节点名描述。
    /// Unique和Require仅支持True,False两个选项，默认为False。
    /// Restrict 校验表达式以正则表达式为基础，扩展其在数值、日期等个方面的匹配范式，逻辑组合关系允许 ["AND" | "OR"] ["NOT"] 三种。具体表达式规则参考相关说明文档。
    /// Alias 别名不支持制表符、换行符,默认的别名同Attr名称一致。
    /// DefaultVal 默认值设定需符和校验表达式，默认的均为空值。
    /// @Notice
    /// 1.未正确设置keyNode的情况下，请注意第一个字段默认为主键字段名称，且必须为非空值。
    /// 2.Enable为可选的附加属性，默认值为True。
    /// 3.配置文档使用UTF-8字符集编码。
    /// @see http://gcm.wfcc.info/GCMDIhelp
    /// </summary>
    public class CustomColDef
    {
        /// <summary>
        /// 属性名称(属性名称可以由大小写字母、数字、下划线组成，字段名大小写敏感)
        /// </summary>
        public string Attr { get; set; }
        /// <summary>
        /// 属性别名
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// 属性值约束条件定义
        /// </summary>
        public string Restrict { get; set; }
        /// <summary>
        /// 唯一性键值约束
        /// </summary>
        public bool IsUnique { get; set; }
        /// <summary>
        /// 必选性键值约束
        /// </summary>
        public bool IsRequire { get; set; }
        /// <summary>
        /// 是否启用的可见字段
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultVal { get; set; }
        /// <summary>
        /// 默认的排序标记值
        /// </summary>
        public int Corder { get; set; }

        internal static List<CustomColDef> getCustomTableDef(string settingPath)
        {
            if (File.Exists(settingPath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                using (FileStream fs = new FileStream(settingPath, FileMode.Open, FileAccess.Read))
                {
                    xmlDoc.Load(fs);
                    XmlNode sxnode = xmlDoc.SelectSingleNode("/CTableConfig/fields");
                    return getCustomTableDef(sxnode);
                }
            }
            else
            {
                log.Fatal("The setting file note exist! @Path=" + settingPath);
                throw new IDCMException("The setting file note exist! @Path=" + settingPath);
            }
        }
        internal static List<CustomColDef> getCustomTableDef(XmlNode sxnode)
        {
            if (sxnode == null)
                return null;
            List<CustomColDef> ctcds = new List<CustomColDef>();
            foreach (XmlNode attrNode in sxnode.ChildNodes)
            {
                CustomColDef ccd = parseCustomTableDef(attrNode);
                if (ccd != null)
                {
                    ccd.Corder = ctcds.Count;
                    ctcds.Add(ccd);
                }
            }
            return ctcds;
        }
        private static CustomColDef parseCustomTableDef(XmlNode attrNode)
        {
            if (attrNode != null)
            {
                CustomColDef ccd = new CustomColDef();
                ccd.Attr = attrNode.Name;
                XmlAttribute aliasAttr = attrNode.Attributes["Alias"];
                if (aliasAttr != null && aliasAttr.Value.Length > 0)
                    ccd.Alias = aliasAttr.Value;
                else
                    ccd.Alias = ccd.Attr;
                foreach (XmlNode subNode in attrNode.ChildNodes)
                {
                    if (subNode.Name.Equals("Restrict"))
                    {
                        ccd.Restrict = subNode.InnerText;
                        continue;
                    }
                    if (subNode.Name.Equals("Unique"))
                    {
                        bool btag=false;
                        Boolean.TryParse(subNode.InnerText, out btag);
                        ccd.IsUnique = btag;
                        continue;
                    }
                    if (subNode.Name.Equals("Require"))
                    {
                        bool btag = false;
                        Boolean.TryParse(subNode.InnerText, out btag);
                        ccd.IsRequire = btag;
                        continue;
                    }
                    if (subNode.Name.Equals("Enable"))
                    {
                        bool btag = true;
                        Boolean.TryParse(subNode.InnerText, out btag);
                        ccd.IsEnable = btag;
                        continue;
                    }
                    if (subNode.Name.Equals("DefaultVal"))
                    {
                        ccd.DefaultVal = subNode.InnerText;
                        continue;
                    }
                }
                return ccd;
            }
            return null;
        }
        public XmlElement toXmlElement(XmlDocument xmlDoc)
        {
            XmlElement field = xmlDoc.CreateElement(Attr);
            if (Alias != null)
            {
                XmlAttribute aliasAttr = xmlDoc.CreateAttribute("Alias");
                aliasAttr.Value = Alias;
                field.Attributes.Append(aliasAttr);
            }
            XmlElement require = xmlDoc.CreateElement("Require");
            require.InnerText = IsRequire.ToString();
            field.AppendChild(require);
            XmlElement unique = xmlDoc.CreateElement("Unique");
            unique.InnerText = IsUnique.ToString();
            field.AppendChild(unique);
            if (Restrict != null)
            {
                XmlElement restrict = xmlDoc.CreateElement("Restrict");
                restrict.InnerText = Restrict.ToString();
                field.AppendChild(restrict);
            }
            if (DefaultVal != null)
            {
                XmlElement defaultVal = xmlDoc.CreateElement("DefaultVal");
                defaultVal.InnerText = DefaultVal;
                field.AppendChild(defaultVal);
            }
            XmlElement enable = xmlDoc.CreateElement("Enable");
            enable.InnerText = IsEnable.ToString();
            field.AppendChild(enable);
            return field;
        }
        public override string ToString()
        {
            return Attr + splitChar + IsRequire + splitChar + IsUnique + splitChar
                + Restrict + splitChar + Alias + splitChar + (DefaultVal == null ? "" : DefaultVal);
        }
        private static readonly char splitChar ='\t';
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
