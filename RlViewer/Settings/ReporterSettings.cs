using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;

namespace RlViewer.Settings
{
    [DataContract]
    public class ReporterSettings : Settings
    {
        
        public ReporterSettings()
        { }

        public ReporterSettings(Behaviors.ReportGenerator.Abstract.ReporterTypes reporterType, int firstLineOffset, int lastLineOffset, bool readToEnd,
            bool addArea, bool addCenter, bool addCorners, bool addParametersTable, bool addTimes)
        {
            ReporterType = reporterType;
            FirstLineOffset = firstLineOffset;
            LastLineOffset = lastLineOffset;
            ReadToEnd = readToEnd;
            AddCorners = addCorners;
            AddCenter = addCenter;
            AddArea = addArea;
            AddParametersTable = addParametersTable;
            AddTimes = addTimes;
        }

        protected override string SettingsPath
        {
            get
            {
                var settingsPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "reporterSettings");
                var fileName = Path.ChangeExtension(settingsPath, SettingsExtension);
                return fileName;
            }
        }


        public Behaviors.ReportGenerator.Abstract.ReporterTypes ReporterType
        {
            get;
            set;
        }
       

        [DataMember]
        public int FirstLineOffset
        {
            get;
            set;
        }


        [DataMember]
        public int LastLineOffset
        {
            get;
            set;
        }

        [DataMember]
        public bool ReadToEnd
        {
            get;
            set;
        }

        [DataMember]
        public bool AddCorners
        {
            get;
            set;
        }

        [DataMember]
        public bool AddCenter
        {
            get;
            set;
        }


        [DataMember]
        public bool AddArea
        {
            get;
            set;
        }


        [DataMember]
        public bool AddParametersTable
        {
            get;
            set;
        }


        [DataMember]
        public bool AddTimes
        {
            get;
            set;
        }
    }
}
