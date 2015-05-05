using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.Base.Utils;
using Newtonsoft.Json;

namespace IDCM.DataTransfer
{
    class MDIDataImporter
    {
        internal static bool parseMDIData(Core.CTableCache ctcache, string mdiPath)
        {
            String jsonStr = FileUtil.readAsUTF8Text(mdiPath);
            List<Dictionary<string, string>> proInfo = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonStr);
            if (proInfo != null)
            {
                foreach(Dictionary<string, string> drow in proInfo)
                {
                    lock (ctcache.GSyncRoot)
                    {
                        ctcache.addRow(drow);
                    }
                }
                return true;
            }
            return false;
        }

    }
}
