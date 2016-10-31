using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.FilesAggregator
{
    class AggregatorParams
    {
        public AggregatorParams(string aggregateFileName, params string[] sourceFilesNames)
        {
            _aggregateFileName = aggregateFileName;
            _sourceFilesNames = sourceFilesNames;
        }

        private string _aggregateFileName;
        public string AggregateFileName
        {
            get 
            {
                return _aggregateFileName; 
            }
        }

        private string[] _sourceFilesNames;
        public string[] SourceFilesNames
        {
            get
            {
                return _sourceFilesNames;
            }
        }
    }
}
