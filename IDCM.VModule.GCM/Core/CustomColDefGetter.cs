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
                if (File.Exists(hisCfg))
                {
                    string[] lines = FileUtil.readAsUTF8Text(hisCfg).Split(new char[] { '\n', '\r' });
                    string hisCode = lines.Length > 0 && lines[0].Length > 0 ? lines[0].Substring(1).Trim() : null;
                    if (File.Exists(userCfg))
                    {
                        FileStream fs= new FileStream(userCfg, FileMode.Open);
                        srcHashCode = HashUtil.md5HexCode(fs);
                        fs.Close();
                    }
                    if (srcHashCode == null || srcHashCode.Equals(hisCode))
                    {
                        ccds = CustomColDef.parseCustomTableDef(lines);
                    }
                }
                if(ccds==null)
                {
                    ccds = CustomColDef.getCustomTableDef(userCfg);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("#").Append(srcHashCode).AppendLine().AppendLine(">>Def");
                    foreach (CustomColDef ccd in ccds)
                    {
                        sb.AppendLine(ccd.ToString());
                    }
                    FileUtil.writeToUTF8File(hisCfg, sb.ToString());
                }
            }
            catch (Exception ex)
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
        private static Dictionary<string,CustomColDef> ccdCache = null;
    }
}
