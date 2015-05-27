using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using IDCM.Base;
using Microsoft.Win32;
using System.Diagnostics;

namespace IDCM.DataTransfer
{
    /// <summary>
    /// 基于外部服务器提供帮助支持的页面请求实现类
    /// </summary>
    public class HelpDocRequester
    {
        #region Methods

        /// <summary>
        /// 通过本地浏览器请求远程帮助文档
        /// 说明:
        /// 1.具体请求的Url为三段式组成，表示为{helpBase}/{requestCatalog}#{requestTag}
        /// </summary>
        /// <param name="requestBase"></param>
        /// <param name="requestTag">页面锚标记(null able)</param>
        /// <param name="requestCatalog">页面分类目录设定(null able)</param>
        public static void requestHelpDoc(string requestBase=null,string requestTag = null, string requestCatalog = null)
        {
            string helpBase = (requestBase != null && requestBase.Length > 0)?requestBase:ConfigurationManager.AppSettings.Get(SysConstants.HelpBase);
            string appTag = (requestTag != null && requestTag.Length > 0) ? "#" + requestTag : "";
            string appCat = (requestCatalog != null && requestCatalog.Length > 0) ? "/" + requestCatalog : "";
            tryToOpenLinkUrl(helpBase + appCat + appTag);
        }

        public static void tryToOpenLinkUrl(string gotoUrl)
        {
            string BrowserPath = GetDefaultWebBrowserFilePath();
            if (BrowserPath == null)
                BrowserPath = getIEFilePath();
            if (BrowserPath != null)
            {
                if (gotoUrl.StartsWith("www"))
                {
                    gotoUrl = "http://" + gotoUrl;
                }
                System.Diagnostics.Process.Start(BrowserPath, gotoUrl);
            }
        }
        /// <summary>
        /// 获取默认浏览器的路径
        /// </summary>
        /// <returns></returns>
        private static String GetDefaultWebBrowserFilePath()
        {
            try
            {
                string _BrowserKey1 = @"Software\Clients\StartmenuInternet\";
                string _BrowserKey2 = @"\shell\open\command";

                RegistryKey _RegistryKey = Registry.CurrentUser.OpenSubKey(_BrowserKey1, false);
                if (_RegistryKey == null)
                    _RegistryKey = Registry.LocalMachine.OpenSubKey(_BrowserKey1, false);
                String _Result = _RegistryKey.GetValue("").ToString();
                _RegistryKey.Close();

                _RegistryKey = Registry.LocalMachine.OpenSubKey(_BrowserKey1 + _Result + _BrowserKey2);
                _Result = _RegistryKey.GetValue("").ToString();
                _RegistryKey.Close();

                if (_Result.Contains("\""))
                {
                    _Result = _Result.TrimStart('"');
                    _Result = _Result.Substring(0, _Result.IndexOf('"'));
                }
                return _Result;
            }
            catch (Exception ex)
            {
                log.Warn("Get RegistryKey of Default browser failed.", ex.StackTrace);
                return null;
            }
        }
        /// <summary>
        /// 获取IE浏览器的路径
        /// </summary>
        /// <returns></returns>
        private static String getIEFilePath()
        {
            return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Internet Explorer\\iexplore.exe");
        }
        #endregion

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
