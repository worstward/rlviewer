using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace RlViewer.Headers.Abstract
{
    public abstract class FileHeader
    {
        /// <summary>
        /// Gets a byte sequence that identifies class of the input file
        /// </summary>
        public abstract byte[] Signature { get; }

        /// <summary>
        /// Gets length of this file header
        /// </summary>
        public abstract int HeaderLength { get; }

        /// <summary>
        /// Returns parsed info from file header
        /// </summary>
        /// <returns>Parsed header info instance</returns>
        public abstract HeaderInfoOutput[] GetHeaderInfo();

        public T ReadStruct<T>(Stream s)
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
            s.Read(buffer, 0, Marshal.SizeOf(typeof(T)));
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            T temp = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return temp;
        }

    }
}
