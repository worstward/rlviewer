using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace RlViewer.Behaviors.Synthesis
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ServerSarErrorMessage
    {
        public int size;
        [MarshalAs(UnmanagedType.I1)]
        public bool error;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
        public byte[] err_buf;
    };
}
