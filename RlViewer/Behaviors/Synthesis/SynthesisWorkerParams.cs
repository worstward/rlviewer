using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Synthesis
{
    class SynthesisWorkerParams
    {
        public SynthesisWorkerParams(Files.Rhg.Abstract.RhgFile[] rhgSeries, string rliFilePath)
        {
            _rhgSeries = rhgSeries;
            _rliFilePath = rliFilePath;
        }

        private Files.Rhg.Abstract.RhgFile[] _rhgSeries;
        public Files.Rhg.Abstract.RhgFile[] RhgSeries
        {
            get 
            {
                return _rhgSeries;
            }
        }

        private string _rliFilePath;
        public string RliFilePath
        {
            get
            {
                return _rliFilePath;
            }
        }
    }
}
