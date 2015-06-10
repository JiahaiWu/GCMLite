using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Core;
using System.IO;

namespace IDCM.DataTransfer
{
    class TextDataImporter
    {
        #region Methods
        internal static bool parseTSVData(CTableCache ctcache, string txtPath, ref Dictionary<string, string> dataMapping)
        {
            if (txtPath == null || txtPath.Length < 1)
                return false;
            string fullPath = System.IO.Path.GetFullPath(txtPath);
            using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                using(StreamReader sr=new StreamReader(fs))
                {
                    Dictionary<short, string> convertMapIdxs = null;
                    parseAttrMapIdxs(sr, ref dataMapping, "\t", out convertMapIdxs);
                    parseDataInfo(ctcache, sr, ref convertMapIdxs, "\t");
                }
            }
            return true;
        }

        internal static bool parseCSVData(CTableCache ctcache, string txtPath, ref Dictionary<string, string> dataMapping)
        {
            if (txtPath == null || txtPath.Length < 1)
                return false;
            string fullPath = System.IO.Path.GetFullPath(txtPath);
            using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    Dictionary<short, string> convertMapIdxs = null;
                    parseAttrMapIdxs(sr, ref dataMapping, ",", out convertMapIdxs);
                    parseDataInfo(ctcache, sr, ref convertMapIdxs, ",");
                }
            }
            return true;
        }

        private static bool parseAttrMapIdxs(StreamReader sr, ref Dictionary<string, string> dataMapping, string splitor, out  Dictionary<short, string> convertMapIdxs)
        {
            Dictionary<string, short> attrMapIdxs = new Dictionary<string, short>();
            string line = null;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Length > 1)
                {
                    string[] attrs = line.Split(new string[] { splitor },StringSplitOptions.None);
                    for (short i = 0; i < attrs.Length;i++ )
                    {
                        attrMapIdxs[attrs[i]] = i;
                    }
                    break;
                }
            }
            if (attrMapIdxs.Count > 0)
            {
                convertMapIdxs = new Dictionary<short, string>();
                foreach (KeyValuePair<string, string> pair in dataMapping)
                {
                    short fid = -1;
                    if (attrMapIdxs.TryGetValue(pair.Key, out fid) && pair.Value != null && pair.Value.Length > 0)
                        convertMapIdxs[fid] = pair.Value;
                }
                return true;
            }
            convertMapIdxs = null;
            return false;
        }

        private static void parseDataInfo(CTableCache ctcache, StreamReader sr, ref Dictionary<short, string> convertMapIdxs, string splitor)
        {
            if (convertMapIdxs != null && convertMapIdxs.Count > 0)
            {
                string line = null;
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (line.Length > 1)
                    {
                        string[] vals = line.Split(new string[] { splitor }, StringSplitOptions.None);
                        if (vals == null || vals.Length<1)
                            break;
                        lock (ctcache.GSyncRoot)
                        {
                            Dictionary<string, string> mapValues = new Dictionary<string, string>();
                            foreach (KeyValuePair<short, string> kv in convertMapIdxs)
                            {
                                if (kv.Key<vals.Length)
                                {
                                    if (vals[kv.Key] != null)
                                    {
                                        string cellData = vals[kv.Key];
                                        mapValues[kv.Value] = cellData;
                                    }
                                }
                            }
                            ctcache.addRow(mapValues);
                        }
                    }
                }
            }
        }
        #endregion

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
