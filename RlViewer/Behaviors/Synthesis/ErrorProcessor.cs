using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Synthesis
{
    class ErrorProcessor
    {

        public void LogErrors(System.IO.Stream errorStream)
        {
            var errors = Behaviors.Converters.StructIO.ReadStruct<Behaviors.Synthesis.ServerSarErrorMessage>(errorStream);
            Logging.Logger.Log(Logging.SeverityGrades.Internal, 
                string.Format("Hologram passed with message: {0}", Encoding.UTF8.GetString(errors.err_buf).TrimEnd('\0')),
                type: Logging.LogType.Synthesis);
            errorStream.Position = 0;
        }
    }
}
