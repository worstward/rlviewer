using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace RlViewer
{

    [DataContract]
    public abstract class XmlSerialized
    {

        /// <summary>
        /// Gets settings from xml file
        /// </summary>
        public static T LoadData<T>() where T : XmlSerialized, new()
        {
            T loadedClass;

            try
            {
                loadedClass = new T().FromXml<T>();
            }
            catch (Exception)
            {
                Logging.Logger.Log(Logging.SeverityGrades.Warning, string.Format("{0} file is corrupted, loading default values", typeof(T)));
                loadedClass = new T();
                loadedClass.ToXml<T>();
            }

            return loadedClass;
        }

        protected abstract string SavingPath
        {
            get;
        }

        protected string SavingExtension
        {
            get
            {
                return "xml";
            }
        }


        public void ToXml<T>() where T : XmlSerialized
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(T));
            var xmlSettings = new XmlWriterSettings() { Indent = true };

            using (var stream = XmlWriter.Create(SavingPath, xmlSettings))
            {
                dcs.WriteObject(stream, this);
            }
        }

        private T FromXml<T>() where T : XmlSerialized
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(T));

            using (var stream = File.OpenRead(SavingPath))
            {
                return (T)dcs.ReadObject(stream);
            }
        }


    }
}
