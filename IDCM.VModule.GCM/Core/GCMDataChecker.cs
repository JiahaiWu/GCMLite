using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Base;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Xml.Schema;

namespace IDCM.Core
{
    class GCMDataChecker
    {
        public static bool checkForPublish(MemoryStream xmlStream,out string errorInfo)
        {
            StringBuilder resStrbuilder = new StringBuilder();
            if (GCMXMLSetting == null)
            {
                string schemaFile = ConfigurationManager.AppSettings.Get(SysConstants.GCMUploadSchema);
                GCMXMLSetting = new XmlReaderSettings();
                GCMXMLSetting.ValidationType = ValidationType.Schema;
                GCMXMLSetting.Schemas.Add(null, schemaFile);
                GCMXMLSetting.ValidationEventHandler += (object sender, ValidationEventArgs e) =>
                {
                    resStrbuilder.Append(e.Message).AppendLine();
                    GCMXMLSetting = null;
                    throw new IDCMException("Error in reading schema in checkForPublish(...)");
                };
            }
            if (GCMXMLSetting != null)
            {
                using (XmlReader reader = XmlReader.Create(xmlStream, GCMXMLSetting))
                {
                    try
                    {
                        while (reader.Read()) ;
                    }
                    catch (XmlException ex)
                    {
                        resStrbuilder.Append(ex.Message).AppendLine();
                    }
                }
            }
            errorInfo = resStrbuilder.ToString();
            return errorInfo.Length < 1;
        }
        private static XmlReaderSettings GCMXMLSetting = null;
    }
}
