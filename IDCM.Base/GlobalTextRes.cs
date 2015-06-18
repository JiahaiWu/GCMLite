using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Globalization;
using System.Configuration;

namespace IDCM.Base
{

    public sealed class GlobalTextRes
    {
        public static string Text(string text)
        {
            CultureInfo ci = (appCI != null ? appCI : Thread.CurrentThread.CurrentCulture);
            string itext= rm.GetString(text, ci);
            return itext==null || itext.Length <1 ? text:itext;
        }
        public static string FindText(string text, CultureInfo ci)
        {
            string itext = rm.GetString(text, ci);
            return itext == null || itext.Length < 1 ? null : itext;
        }
        public static string getLanguageName()
        {
            return (appCI!=null?appCI:Thread.CurrentThread.CurrentCulture).Name;
        }
        public static void noteApplicationCulture(CultureInfo cultureInfo)
        {
            appCI = cultureInfo;
        }

        /// <summary>
        /// 内部默认的字符编码
        /// </summary>
        public static Encoding DataEncoding
        {
            get
            {
                if (dataEncoding != null)
                    return dataEncoding;
                else
                {
                    try
                    {
                        dataEncoding = Encoding.GetEncoding(ConfigurationManager.AppSettings[SysConstants.DataEncoding]);
                    }
                    catch (Exception)
                    {
                        dataEncoding = null;
                    }
                    dataEncoding = SysConstants.defaultEncoding;
                }
                return dataEncoding;
            }
            set
            {
                dataEncoding = value;
            }
        }

        private static Encoding dataEncoding =null;
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static CultureInfo appCI = null;
        private static ResourceManager rm = new ResourceManager("IDCM.Base.TextResources", Assembly.GetExecutingAssembly());
    }
}
