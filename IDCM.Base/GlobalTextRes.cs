using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Globalization;

namespace IDCM.Base
{
    public sealed class GlobalTextRes
    {
        public static string Text(string text)
        {
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            string itext= rm.GetString(text, ci);
            return itext.Length > 0 ? itext : text;
        }
        private static ResourceManager rm = new ResourceManager("IDCM.Base.TextResources", Assembly.GetExecutingAssembly());
    }
}
