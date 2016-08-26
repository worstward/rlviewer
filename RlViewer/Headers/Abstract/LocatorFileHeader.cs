using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace RlViewer.Headers.Abstract
{

    /// <summary>
    /// Incapsulates radiolocation file
    /// </summary>
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


        /// <summary>
        /// Size of 1 point in a file
        /// </summary>
        public abstract int BytesPerSample { get; }

        /// <summary>
        /// Size of a data header
        /// </summary>
        public abstract int StrHeaderLength { get; }

        /// <summary>
        /// Contains file info in a readable format
        /// </summary>
        public abstract HeaderInfoOutput[] HeaderInfo { get; }

        /// <summary>
        /// Returns parsed info from file header
        /// </summary>
        /// <returns>Array of parsed subheaders info</returns>
        protected abstract HeaderInfoOutput[] GetHeaderInfo();


        /// <summary>
        /// Reads file header from provided path and converts it to corresponding header structure
        /// </summary>
        /// <typeparam name="T">Header structure type</typeparam>
        /// <param name="path">path of location file</param>
        /// <returns>Header structure</returns>
        protected T ReadHeader<T>(string path) where T : struct
        {
            byte[] header = new byte[FileHeaderLength];

            using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Read(header, 0, header.Length);
            }

            return Behaviors.Converters.StructIO.ReadStruct<T>(header);
        }


        /// <summary>
        /// Checks if file really belongs to its extension type
        /// </summary>
        /// <param name="header">Full header file</param>
        /// <returns>Returns true if file is one of a checked type, false otherwise</returns>
        protected virtual bool CheckSignature(byte[] fileSignature)
        {
            for (int i = 0; i < Signature.Length; i++)
            {
                if (fileSignature[i] != Signature[i])
                {
                    return false;
                }
            }

            return true;
        }





    }
}
