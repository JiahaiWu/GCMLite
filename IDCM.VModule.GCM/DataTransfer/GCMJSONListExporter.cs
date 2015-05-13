using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Core;
using System.IO;
using IDCM.Base;
using Newtonsoft.Json;
using IDCM.Base.AbsInterfaces;

namespace IDCM.DataTransfer
{
    class GCMJSONListExporter
    {
        public bool exportJSONList(GCMTableCache gtCache, string filepath)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                int tcount = gtCache.getOverViewRowCount();
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    Dictionary<string, int> maps = gtCache.getOverViewIAttrMapping();
                    //填写内容////////////////////
                    int ridx = 0;
                    while (ridx < tcount)
                    {
                        Dictionary<int, string> drow = gtCache.getOverViewIRow(ridx);
                        if (drow != null)
                        {
                            string jsonStr = convertToJsonStr(maps, drow);
                            strbuilder.Append(jsonStr).Append("\r\n");
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
                throw new IDCMException("Error raised in exportJSONList(...)", ex);
            }
            return true;
        }
        public bool exportJSONList(GCMTableCache ctableCache, string filepath, int[] selectedRows)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                int count = 0;
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    Dictionary<string, int> maps = ctableCache.getOverViewIAttrMapping();
                    //填写内容////////////////////
                    for (int ridx = selectedRows.Length - 1; ridx >= 0; ridx--)
                    {
                        int lindex = selectedRows[ridx];
                        Dictionary<int, string> drow = ctableCache.getOverViewIRow(lindex);
                        if (drow != null)
                        {
                            string jsonStr = convertToJsonStr(maps, drow);
                            strbuilder.Append(jsonStr).Append("\r\n");
                            /////////////
                            if (++count % 100 == 0)
                            {
                                Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                                BinaryWriter bw = new BinaryWriter(fs);
                                fs.Write(info, 0, info.Length);
                                strbuilder.Length = 0;
                            }
                        }
                    }
                    if (strbuilder.Length > 0)
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                        BinaryWriter bw = new BinaryWriter(fs);
                        fs.Write(info, 0, info.Length);
                        strbuilder.Length = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new IDCMException("Error raised in exportJSONList(...)", ex);
            }
            return true;
        }

        protected string convertToJsonStr(Dictionary<string, int> maps, Dictionary<int, string> vals)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (KeyValuePair<string, int> kvpair in maps)
            {
                if (kvpair.Value > 0)
                {
                    string key = kvpair.Key;
                    int k = kvpair.Value;
                    dict[key] = vals[k];
                }
            }
            return JsonConvert.SerializeObject(dict);
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
