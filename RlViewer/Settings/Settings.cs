using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace RlViewer.Settings
{

    [DataContract]
    public abstract class Settings
    {

        /// <summary>
        /// Gets settings from xml file
        /// </summary>
        public static T LoadSettings<T>() where T : Settings, new()
        {
            T settings;

            try
            {
                settings = new T().FromXml<T>();
            }
            catch (Exception)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Warning, string.Format("{0} file is corrupted, loading default values", typeof(T)));
                settings = new T();
                settings.ToXml<T>();
            }

            return settings;
        }

        protected abstract string SettingsPath
        {
            get;
        }

        protected string SettingsExtension
        {
            get
            {
                return "xml";
            }
        }


        public void ToXml<T>() where T : Settings
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(T));
            var xmlSettings = new XmlWriterSettings() { Indent = true };

            using (var stream = XmlWriter.Create(SettingsPath, xmlSettings))
            {
                dcs.WriteObject(stream, this);
            }
        }

        private T FromXml<T>() where T : Settings
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(T));

            using (var stream = File.OpenRead(SettingsPath))
            {
                return (T)dcs.ReadObject(stream);
            }
        }


    }
}
