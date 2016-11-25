using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Synthesis.SystemEvents
{
    public class SystemEvents
    {
        public SystemEvents(int timeOut)
        {
            _timeOut = timeOut;
        }

        private int _timeOut;

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

        public void WaitHologramReady(int sharedMemoryNum)
        {
            WaitEvent(string.Format("Hol_Ready_{0}", sharedMemoryNum));
        }

        public void WaitRhgReading(int sharedMemoryNum)
        {
            WaitEvent(string.Format("ReadRhg", sharedMemoryNum));
        }

        public void RhgReading(int sharedMemoryNum)
        {
            FireEvent(string.Format("ReadRhg", sharedMemoryNum));
        }

        public void WaitSharedRhgMemoryFree(int sharedMemoryNum)
        {
            WaitEvent(string.Format("SharedMemoryFree", sharedMemoryNum));
        }


        public void SharedRhgMemoryFree(int sharedMemoryNum)
        {
            FireEvent(string.Format("SharedMemoryFree", sharedMemoryNum));
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
