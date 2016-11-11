using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Synthesis
{
    class ServerSarInterop
    {
        public ServerSarInterop(string serverSarPath, ServerSarTaskParams sstp)
        {
            _serverSarPath = serverSarPath;
            _sstp = sstp;
        }

        private string _serverSarPath;
        private ServerSarTaskParams _sstp;


        public void StartSynthesis()
        {
            
            using (var mf = new Behaviors.Synthesis.SharedMemory.MappedFile("SSTP_inSharedMem", (long)(new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory / 20)))
            {
                var sstpBytes = RlViewer.Behaviors.Converters.StructIO.WriteStruct<RlViewer.Behaviors.Synthesis.ServerSarTaskParams>(_sstp);
                mf.WriteData(sstpBytes);
                SystemEvents.SystemEvents.SstpReady();
                System.Diagnostics.Process.Start(_serverSarPath, "-1 -2");
            }

           
        }

    }
}
