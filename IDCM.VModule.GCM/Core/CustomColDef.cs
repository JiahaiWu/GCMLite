using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IDCM.Base.Utils;
using IDCM.Base;

namespace IDCM.Core
{
    /// <summary>
    /// IDCM Custom Table Definition
    /// @Date 2014-09-20
    /// @author JiahaiWu 
    /// @Description:
    /// 字段名称	必须非空	必须唯一	校验表达式	别名	默认值
    /// 对应于
    /// attr	isRequire	isUnique	restrict	Alias	defaultVal
    /// 使用制表符\t作为分隔字符。
    /// 以#号开头行为注释行。
    /// 以>>Def Ver 标识表属性定义重置起点及版本定义。
    /// 以方括号[]包含标识分组，括号内字符串表示分组名称。
    ///
    /// attr 字段名称不支持制表符、换行符。
    /// isUnique和IsRequire仅支持True,False两个选项，默认为False。
    /// restrict 校验表达式以正则表达式为基础，扩展其在数值、日期、文件路径四个方面的匹配范式。具体表达式可由["REGEX:" regex_expr | "DATE:" date_expr | "NUMBER:" number_expr | "PATH:" path_expr] 四种部分进行逻辑组合，逻辑组合关系允许 ["AND" | "OR"] ["NOT"] 三种。
    /// Alias 别名不支持制表符、换行符。
    /// defaultVal 默认值设定需符和校验表达式，默认的均为空值。
    /// 本文档请使用UTF-8字符集编码。
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
        /// 默认值
        /// </summary>
        public string DefaultVal { get; set; }
        /// <summary>
        /// 默认的排序标记值
        /// </summary>
        public int Corder { get; set; }


        internal static List<CustomColDef> getCustomTableDef(string settingPath)
        {
            List<CustomColDef> ctcds = new List<CustomColDef>();
            if (File.Exists(settingPath))
            {
                string[] lines = FileUtil.readAsUTF8Text(settingPath).Split(new char[] { '\n', '\r' });
                return parseCustomTableDef(lines);
            }
            else
            {
                log.Fatal("The setting file note exist! @Path=" + settingPath);
                throw new IDCMException("The setting file note exist! @Path=" + settingPath);
            }
        }
        internal static List<CustomColDef> parseCustomTableDef(string[] lines)
        {
            List<CustomColDef> ctcds = new List<CustomColDef>();
            foreach (string line in lines)
            {
                if (line.Length < 1 || line.StartsWith("#"))
                    continue;
                if (line.StartsWith("[") && line.TrimEnd().EndsWith("]"))
                    continue;
                if (line.StartsWith(">>Def"))
                {
                    if (line.Length > 9)
                    {
                        string ver = line.Substring(9).Trim();
                    }
                    ctcds.Clear();
                    continue;
                }
                CustomColDef ctcd = formatSettingLine(line);
                if (ctcd != null && ctcd.Attr.Length>0)
                {
                    ctcds.Add(ctcd);
                    //set default col order for ctcd attr
                    ctcd.Corder = ctcds.Count;
                }
            }
            return ctcds;
        }
        internal static CustomColDef formatSettingLine(string line)
        {
            string[] vals = line.Split(splitChars);
            if (vals.Length > 0)
            {
                CustomColDef ctcd = new CustomColDef();
                ctcd.Attr =vals[0];
                ctcd.IsRequire = vals.Length > 1 ? Convert.ToBoolean(vals[1]) : false;
                ctcd.IsUnique = vals.Length > 2 ? Convert.ToBoolean(vals[2]) : false;
                ctcd.Restrict = vals.Length > 3? vals[3] : "";
                ctcd.Alias = vals.Length > 4 ? vals[4] : "";
                ctcd.Alias = ctcd.Alias.Length > 0 ? ctcd.Alias : ctcd.Attr;
                ctcd.DefaultVal = vals.Length > 5 ? vals[5] : "";
                return ctcd;
            }
            return null;
        }
        public override string ToString()
        {
            return Attr + splitChars[0] + IsRequire + splitChars[0] + IsUnique + splitChars[0]
                + Restrict + splitChars[0] + Alias + splitChars[0] + (DefaultVal == null ? "" : DefaultVal);
        }
        private static readonly char[] splitChars =new char[] {'\t' };
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
