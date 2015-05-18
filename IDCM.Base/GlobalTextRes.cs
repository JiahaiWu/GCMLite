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
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            string itext= rm.GetString(text, ci);
            return itext==null || itext.Length <1 ? text:itext;
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private static ResourceManager rm = new ResourceManager("IDCM.Base.TextResources", Assembly.GetExecutingAssembly());
    }
}
