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

            
            var sstpMf = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateNew("SSTP_inSharedMem", (long)(new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory / 20));

            var sstpBytes = RlViewer.Behaviors.Converters.StructIO.WriteStruct<RlViewer.Behaviors.Synthesis.ServerSarTaskParams>(_sstp);

            var wr = sstpMf.CreateViewStream();
            wr.Write(sstpBytes, 0, sstpBytes.Length);

            var errorMf = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateNew("ErrMes_inSharedMem_0", 4102);
            var dspMf = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateNew("DSP_inSharedMem_0", 1048577);
            var holMf = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateNew("Hol_inSharedMem_0", (long)(_sstp.Mlength * _sstp.Times_mc * _sstp.Nlength));
            var rliMf = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateNew("Rli_inSharedMem_0", (long)(_sstp.Nshift * _sstp.Nscale * _sstp.Mshift * _sstp.Mscale));
           
            System.Threading.EventWaitHandle handle = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset, "Hol_Ready_0");
            handle.Set();
           
            System.Threading.EventWaitHandle handle444 = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset, "Res_Ready");
            handle444.Set();

            System.Diagnostics.Process.Start(_serverSarPath, "-1 -2");
            SystemEvents.SystemEvents.SstpReady();


            //var dspMf = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateNew("DSP_inSharedMem[0]", 1048576);

            //var holMf = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateNew("Hol_inSharedMem[0]",(long)(_sstp.Mlength * _sstp.Times_mc * _sstp.Nlength));
            //var rliMf = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateNew("Rli_inSharedMem[0]", (long)(_sstp.Nshift * _sstp.Nscale * _sstp.Mshift * _sstp.Mscale));

        }

    }
}
