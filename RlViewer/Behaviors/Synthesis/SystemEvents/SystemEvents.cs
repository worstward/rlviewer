using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Synthesis.SystemEvents
{
    public static class SystemEvents
    {

        public static void SstpReady()
        {
            FireEvent("SSTP_Ready");
        }

        public static void HologramReady()
        {
            FireEvent("Hol_Ready");
        }

        public static void RliReady()
        {
            FireEvent("Rli_Ready");
        }

        
        private static void FireEvent(string eventName)
        {
            System.Threading.EventWaitHandle handle = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset, eventName);
            handle.Set();
        }

    }
}
