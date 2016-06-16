using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ReportGenerator.Abstract
{
    public abstract class Reporter
    {
        public Reporter(params string[] filePaths)
        {
            _filePaths = filePaths;
        }

        private string[] _filePaths;
        protected string[] FilePaths
        {
            get
            {
                return _filePaths;
            }
        }

        public abstract void GenerateReport(string reportFilePath);
    
    }
}
