using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.Base;
using System.IO;
using System.Configuration;
using IDCM.Base.Utils;

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
                string userCfg = ConfigurationManager.AppSettings[SysConstants.CTableDef];
                if (File.Exists(userCfg))
                {
                    FileStream fs = new FileStream(userCfg, FileMode.Open);
                    srcHashCode = HashUtil.md5HexCode(fs);
                    fs.Close();
                }
                if (File.Exists(hisCfg))
                {
                    string[] lines = FileUtil.readAsUTF8Text(hisCfg).Split(new char[] { '\n', '\r' });
                    lastSrcHashCode = lines.Length > 0 && lines[0].Length > 0 ? lines[0].Substring(1).Trim() : null;
                    if (srcHashCode == null || srcHashCode.Equals(lastSrcHashCode))
                    {
                        ccds = CustomColDef.parseCustomTableDef(lines);
                    }
                }
                if(ccds==null)
                {
                    if (!File.Exists(userCfg))
                        throw new IDCMException("缺少数据表配置文件！ @path="+ userCfg);
                    ccds = CustomColDef.getCustomTableDef(userCfg);
                    lastSrcHashCode = srcHashCode;
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

        public static bool saveUpdatedHistCfg()
        {
            if(dirtyStatus)
            {
                dirtyStatus=true;
                string cacheDir = SysConstants.initEnvDir + SysConstants.cacheDir;
                if (!File.Exists(cacheDir))
                    Directory.CreateDirectory(cacheDir);
                string hisCfg = cacheDir + SysConstants.tableDefNote;
                StringBuilder sb = new StringBuilder();
                sb.Append("#").Append(lastSrcHashCode).AppendLine().AppendLine(">>Def");
                foreach (CustomColDef ccd in ccdCache.Values)
                {
                    sb.AppendLine(ccd.ToString());
                }
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
        internal static void updateCustomColDef(string attr, bool isRequired)
        {
            CustomColDef ccd = null;
            if (ccdCache.TryGetValue(attr,out ccd))
            {
                if(ccd.IsRequire != isRequired)
                {
                    ccd.IsRequire = isRequired;
                    dirtyStatus = true;
                }
                saveUpdatedHistCfg();
            }
        }
        private volatile static bool dirtyStatus = false;
        private static string lastSrcHashCode=null;
        private static Dictionary<string,CustomColDef> ccdCache = null;
    }
}
