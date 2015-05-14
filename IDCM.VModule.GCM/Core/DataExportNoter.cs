using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace IDCM.Core
{
    class DataExportNoter
    {
        #region Methods

        /// <summary>
        /// 加载历史导出样本ID记录
        /// </summary>
        public static void loadHistorySIds(string filepath)
        {
            lock (sampleIds)
            {
                if (!File.Exists(filepath))
                    return;
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    sampleIds.Add(line.Trim());
                }
                sr.Close();
                fs.Close();
            }
        }
        /// <summary>
        /// 存储导出数据记录。
        /// 开启后台线程执行文件写入操作。
        /// </summary>
        public static void dumpExportSampleIds(string filepath, ICollection<string> appendSampleIds)
        {
            lock (sampleIds)
            {
                foreach (string id in appendSampleIds)
                {
                    sampleIds.Add(id);
                }
                lock (paramedThreadStart)
                {
                    Thread dumpThread = new Thread(paramedThreadStart);
                    dumpThread.Start(filepath);
                    dumpThread.Name = "Export_Dump_Thread";
                }
            }
        }

        /// <summary>
        /// 将缓存记录写入到文本文件
        /// </summary>
        /// <param name="filepath"></param>
        protected static void dumpToFile(object filepath)
        {
            FileStream fs = new FileStream((string)filepath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, new UTF8Encoding(true));
            foreach (string id in sampleIds)
            {
                sw.WriteLine(id);
            }
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        /// <summary>
        /// 获取特定sample id 是否已经被导出过
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static bool isExported(string sid)
        {
            if (sampleIds.Contains(sid))
                return true;
            return false;
        }
        #endregion

        #region Members

        private static ParameterizedThreadStart paramedThreadStart = new ParameterizedThreadStart(dumpToFile);
        private static HashSet<String> sampleIds = new HashSet<string>();
        #endregion
    }
}
