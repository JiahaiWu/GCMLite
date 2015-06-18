using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Base.Utils;
using Newtonsoft.Json;
using System.IO;
using IDCM.Base;

namespace IDCM.DataTransfer
{
    class JSONListDataImporter
    {
        #region Methods
        internal static bool parseJSONListData(Core.CTableCache ctcache, string jsoPath,ref Dictionary<string, string> dataMapping)
        {
            if (!System.IO.File.Exists(jsoPath))
                return false;
            using (FileStream fs = new FileStream(jsoPath, FileMode.Open, FileAccess.Read))
            {
                StreamReader sr = new StreamReader(fs, GlobalTextRes.DataEncoding, true);
                string line = sr.ReadLine();
                while (line != null)
                {
                    if (line.Length > 1)
                    {
                        Dictionary<string, string> drow = JsonConvert.DeserializeObject<Dictionary<string, string>>(line);
                        if (dataMapping != null)
                        {
                            Dictionary<string, string> rdrow = new Dictionary<string, string>();
                            foreach (KeyValuePair<string, string> kv in drow)
                            {
                                string mkey = null;
                                if (dataMapping.TryGetValue(kv.Key, out mkey) && mkey != null)
                                {
                                    rdrow[mkey] = kv.Value;
                                }
                            }
                            lock (ctcache.GSyncRoot)
                            {
                                ctcache.addRow(rdrow);
                            }
                        }
                        else
                        {
                            lock (ctcache.GSyncRoot)
                            {
                                ctcache.addRow(drow);
                            }
                        }
                    }
                    if (sr.EndOfStream)
                        break;
                    else
                        line = sr.ReadLine();
                }
                sr.Close();
            }
            return true;
        }
        #endregion
    }
}
