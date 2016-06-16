using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.Reporter.Concrete
{
    class DocReporterFactory : Abstract.ReporterFactory
    {
        public override Behaviors.ReportGenerator.Abstract.Reporter Create(params string[] filePaths)
        {
            return new Behaviors.ReportGenerator.Concrete.DocFileReporter(filePaths);
        }
    }
}
