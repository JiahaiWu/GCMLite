using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using IDCM.Base;
using System.Text.RegularExpressions;

namespace IDCM.ViewManager
{
    class ABCServManager
    {
        #region Members
        private DCMControlLib.GCM.ABCBrowser abcBrowser_abc;
        private Regex strainNumberPattern;
        #endregion

        #region Constructor&Destructor
        public ABCServManager(DCMControlLib.GCM.ABCBrowser abcBrowser_abc)
        {
            this.abcBrowser_abc = abcBrowser_abc;
            strainNumberPattern = new Regex(@"([A-Za-z]{2,10})[^A-Za-z0-9]{0,2}(\d{1,10})");
        }
        #endregion

        #region Methods
        internal bool linkTo(string strainId)
        {
            Match match = strainNumberPattern.Match(strainId);
            if (match.Success)
            {
                string queryUri = ConfigurationManager.AppSettings[SysConstants.ABCQueryUri];
                string url = string.Format(queryUri, new string[] { System.Uri.EscapeDataString(match.Groups[1].Value), System.Uri.EscapeDataString(match.Groups[2].Value) });
                abcBrowser_abc.Url = new System.Uri(url);
            }
            else
            {
                string searchUri = ConfigurationManager.AppSettings[SysConstants.ABCSearchUri];
                string url = string.Format(searchUri, new string[] {  System.Uri.EscapeDataString(strainId) });
                abcBrowser_abc.Url = new System.Uri(url);
            }
            return true;
        }
        #endregion
    }
}
