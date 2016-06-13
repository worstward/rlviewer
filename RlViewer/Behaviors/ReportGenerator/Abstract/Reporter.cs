using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ReportGenerator.Abstract
{
    public abstract class Reporter
    {
        public Reporter(Files.LocatorFile file)
        {
            _file = file;
        }

        private Files.LocatorFile _file;
        protected Files.LocatorFile File
        {
            get
            {
                return _file;
            }
        }


        public abstract void GenerateReport(string fileName);


    }
}
