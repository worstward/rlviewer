using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.ReportGenerator.Abstract
{


    public abstract class Reporter : WorkerEventController
    {
        public Reporter(params string[] filesToProcess)
        {
            _filesToProcess = filesToProcess;
        }

        private string[] _filesToProcess;
        protected string[] FilesToProcess
        {
            get
            {
                return _filesToProcess;
            }
        }

        public abstract void GenerateReport(string reportFilePath);
    
    }
}
