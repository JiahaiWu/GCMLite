﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDCM.ComPO;
using System.Configuration;
using IDCM.Base;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace IDCM.DataTransfer
{
    class StrainListQueryExecutor
    {
        #region Methods

        public static StrainListPage strainListQuery(int currentPage, string strainnumber = "", string strainname = "", AuthInfo authInfo = null, int timeout = 10000)
        {
            if (authInfo != null && currentPage >= 0)
            {
                string signInUri = ConfigurationManager.AppSettings[SysConstants.StrainListUri];
                string url = string.Format(signInUri, new object[] { authInfo.Jsessionid, currentPage, System.Uri.EscapeDataString(strainnumber), System.Uri.EscapeDataString(strainname) });
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                log.Info("StrainListQueryExecutor Request Url=" + url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "*/*";
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)";
                request.Timeout = timeout;
                request.ReadWriteTimeout = timeout;
                Stream myRequestStream = request.GetRequestStream();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("UTF-8"));
                string resStr = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                log.Info("StrainListQueryExecutor Response=" + resStr);
                StrainListPage slp = parserToListPageInfo(resStr);
                if (slp != null)
                {
                    slp.Jsessionid = authInfo.Jsessionid;
                }
                return slp;
            }
            return null;
        }

        protected static StrainListPage parserToListPageInfo(string jsonStr)
        {
            StrainListPage slp = JsonConvert.DeserializeObject<StrainListPage>(jsonStr);
            return slp;
        }
        #endregion

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
