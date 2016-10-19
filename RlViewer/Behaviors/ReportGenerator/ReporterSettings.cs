using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ReportGenerator
{
    public class ReporterSettings
    {
        public ReporterSettings(int firstLineOffset, int lastLineOffset, bool readToEnd,
            bool addArea, bool addCenter, bool addCorners, bool addParametersTable, bool addTimes)
        {
            FirstLineOffset = firstLineOffset;
            LastLineOffset = lastLineOffset;
            ReadToEnd = readToEnd;
            AddCorners = addCorners;
            AddCenter = addCenter;
            AddArea = addArea;
            AddParametersTable = addParametersTable;
            AddTimes = addTimes;
        }

        public int FirstLineOffset
        {
            get;
            private set;
        }

        public int LastLineOffset
        {
            get;
            private set;
        }

        public bool ReadToEnd
        {
            get;
            private set;
        }


        public bool AddCorners
        {
            get;
            private set;
        }


        public bool AddCenter
        {
            get;
            private set;
        }

        public bool AddArea
        {
            get;
            private set;
        }

        public bool AddParametersTable
        {
            get;
            private set;
        }

        public bool AddTimes
        {
            get;
            private set;
        }
    }
}
