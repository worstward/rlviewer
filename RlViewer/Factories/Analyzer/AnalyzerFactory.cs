using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.Analyzer
{
    public class AnalyzerFactory
    {
        public static Behaviors.Analyzing.Abstract.SampleAnalyzer Create(Files.LocatorFile file)
        {
            switch (file.Properties.Type)
            {
                case FileType.brl4:
                case FileType.rl4:
                case FileType.raw:
                case FileType.r:
                case FileType.rl8:
                    return new Behaviors.Analyzing.Concrete.FloatSampleAnalyzer();
                case FileType.k:
                    return new Behaviors.Analyzing.Concrete.ShortSampleAnalyzer();
                default:
                    throw new NotSupportedException("Unsupported file format");
            }
        }

    }
}
