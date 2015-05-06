using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCM.ComPO
{
    class StrainListPage
    {
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public string strainname { get; set; }
        public string strainnumber { get; set; }
        public int totalpage { get; set; }
        public string Jsessionid { get; set; }
        public Dictionary<string, string>[] list { get; set; }
    }
}
