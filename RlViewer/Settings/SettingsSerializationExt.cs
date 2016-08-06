using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml;
using System.IO;

namespace RlViewer.Settings
{
    public static class SettingsSerializationExt
    {
        private static string _settingsPath =
            Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "settings");

        
        public static void ToXml(this Settings settings)
        {           
            DataContractSerializer dcs = new DataContractSerializer(typeof(Settings));

            var xmlSettings = new XmlWriterSettings() { Indent = true };

            using (var stream = XmlWriter.Create(Path.ChangeExtension(_settingsPath, "xml"), xmlSettings))
            {
                dcs.WriteObject(stream, settings);
            }

        }

        public static Settings FromXml(this Settings settings)
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(Settings));

            using (var stream = File.OpenRead(Path.ChangeExtension(_settingsPath, "xml")))
            {
                return (Settings)dcs.ReadObject(stream);
            }
        }


        //public static void ToJson(this Settings settings)
        //{
        //    DataContractJsonSerializer dcs = new DataContractJsonSerializer(typeof(Settings));

        //    using (var stream = File.OpenWrite(Path.ChangeExtension(_settingsPath, "json")))
        //    {
        //        dcs.WriteObject(stream, settings);
        //    }
        //}

        //public static Settings FromJson(this Settings settings)
        //{
        //    DataContractJsonSerializer dcs = new DataContractJsonSerializer(typeof(Settings));

        //    using (var stream = File.OpenRead(Path.ChangeExtension(_settingsPath, "json")))
        //    {
        //        return (Settings)dcs.ReadObject(stream);
        //    }
        //}


        
    }
}
