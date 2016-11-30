using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Synthesis.SystemEvents
{
    public class SystemEvents
    {
        public SystemEvents(int timeOut, int memoryParts)
        {
            _timeOut = timeOut;
            _semaphore = new System.Threading.Semaphore(memoryParts, memoryParts);
        }

        private int _timeOut;
        private System.Threading.Semaphore _semaphore;


        public void SstpReady()
        {
            FireEvent("SSTP_Ready");
        }

        public void HologramReady(int sharedMemoryNum)
        {
            FireEvent(string.Format("Hol_Ready_{0}", sharedMemoryNum));
        }


        public void WaitRliReady(int sharedMemoryNum)
        {
            WaitEvent(string.Format("Rli_Ready_{0}", sharedMemoryNum));
        }


        public void WaitFreeMemoryPart()
        {
            _semaphore.WaitOne();
        }

        public void ReleaseMemoryPart()
        {
            _semaphore.Release();
        }


        private void WaitEvent(string eventName)
        {
            System.Threading.EventWaitHandle handle = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset, eventName);
            handle.WaitOne(_timeOut);
        }


        private void FireEvent(string eventName)
        {
            System.Threading.EventWaitHandle handle = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset, eventName);
            handle.Set();
        }

    }
}
