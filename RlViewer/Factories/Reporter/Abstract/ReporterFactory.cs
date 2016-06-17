using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Factories.Reporter.Abstract
{
    public abstract class ReporterFactory
    {
        public abstract Behaviors.ReportGenerator.Abstract.Reporter Create(params string[] filePaths);

        public static ReporterFactory GetFactory(Behaviors.ReportGenerator.Abstract.ReporterTypes reporterType)
        {
            switch(reporterType)
            {
                case Behaviors.ReportGenerator.Abstract.ReporterTypes.Docx:
                    return new Concrete.DocReporterFactory();
                
            }

            throw new NotImplementedException("reporterType");
        }
    }
}
