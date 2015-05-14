using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using IDCM.Core;
using IDCM.Base;
using Newtonsoft.Json;

namespace IDCM.DataTransfer
{
    class JSONListExporter
    {
        #region Methods

        public bool exportJSONList(CTableCache ctableCache, string filepath)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                int tcount = ctableCache.getRowCount();
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    Dictionary<string, int> maps = ctableCache.getIAttrMapping();
                    //填写内容////////////////////
                    int ridx = 0;
                    while (ridx < tcount)
                    {
                        Dictionary<int, string> drow = ctableCache.getIRow(ridx);
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
        public bool exportJSONList(CTableCache ctableCache, string filepath, int[] selectedRows)
        {
            try
            {
                int count = 0;
                StringBuilder strbuilder = new StringBuilder();
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    Dictionary<string, int> maps = ctableCache.getIAttrMapping();
                    //填写内容////////////////////
                    for (int ridx = selectedRows.Length - 1; ridx >= 0; ridx--)
                    {
                        int lindex = selectedRows[ridx];
                        Dictionary<int, string> drow = ctableCache.getIRow(lindex);
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

        protected string convertToJsonStr(Dictionary<string, int> maps, Dictionary<int,string> vals)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (KeyValuePair<string, int> kvpair in maps)
            {
                if (kvpair.Value > -1)
                {
                    string key = kvpair.Key;
                    int k = kvpair.Value;
                    string value = null;
                    if (vals.TryGetValue(k, out value))
                    {
                        dict[key] = value;
                    }
                }
            }
            return JsonConvert.SerializeObject(dict);
        }
        #endregion

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
