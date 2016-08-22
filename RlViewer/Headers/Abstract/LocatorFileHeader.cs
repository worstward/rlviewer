using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace RlViewer.Headers.Abstract
{
    public abstract class LocatorFileHeader
    {
        /// <summary>
        /// Gets a byte sequence that identifies class of the input file
        /// </summary>
        protected abstract byte[] Signature { get; }

        /// <summary>
        /// Gets length of this file header
        /// </summary>
        public abstract int FileHeaderLength { get; }

        public abstract int BytesPerSample { get; }

        public abstract int StrHeaderLength { get; }

        public abstract HeaderInfoOutput[] HeaderInfo { get; }

        /// <summary>
        /// Returns parsed info from file header
        /// </summary>
        /// <returns>Array of parsed subheaders info</returns>
        protected abstract HeaderInfoOutput[] GetHeaderInfo();


        protected T ReadHeader<T>(string path) where T : struct
        {
            byte[] header = new byte[FileHeaderLength];

            using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Read(header, 0, header.Length);
            }

            return Behaviors.Converters.StructIO.ReadStruct<T>(header);
        }



        protected virtual bool CheckSignature(byte[] header)
        {
            for (int i = 0; i < Signature.Length; i++)
            {
                if (header[i] != Signature[i])
                {
                    return false;
                }
            }

            return true;
        }





    }
}
