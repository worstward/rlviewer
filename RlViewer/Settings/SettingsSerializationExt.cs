using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

namespace RlViewer.Settings
{
    public static class SettingsSerializationExt
    {
        private static string _settingsXmlPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "settings.xml");


        public static void ToXml(this Settings settings)
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(Settings));

            try
            {
                var xmlSettings = new XmlWriterSettings() { Indent = true };

                using (var stream = XmlWriter.Create(_settingsXmlPath, xmlSettings))
                {
                    dcs.WriteObject(stream, settings);
                }
            }
            catch
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error, "Error while writing settings to file");
            }
        }

        public static Settings FromXml(this Settings settings)
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(Settings));

            try
            {
                using (var stream = XmlReader.Create(_settingsXmlPath))
                {
                    return (Settings)dcs.ReadObject(stream);
                }
            }
            catch
            {
                Logging.Logger.Log(Logging.SeverityGrades.Error, "Error while reading settings from file");
                throw;
            }

        }


    }
}
