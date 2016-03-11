using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete;
using System.IO;
using System.Runtime.InteropServices;


namespace RlViewer.Files
{
    public abstract class LocatorFile : LoadedFile, IHeader
    {
        protected LocatorFile(FileProperties properties) : base(properties)
        {

        }

        public abstract LocatorFileHeader Header { get; }

        public abstract int Width { get; }
        public abstract int Height { get; }

        public static T ReadStruct<T>(Stream s)
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
            s.Read(buffer, 0, Marshal.SizeOf(typeof(T)));
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            T temp = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return temp;
        }

        public static byte[] WriteStruct<T>(T str)
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }


    }
}
