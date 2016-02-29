using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Headers.Abstract
{
    public abstract class FileHeader
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
        /// <summary>
        /// Returns parsed info from file header
        /// </summary>
        /// <returns>Array of parsed subheaders info</returns>
        public abstract HeaderInfoOutput[] GetHeaderInfo();

        protected virtual void CheckInfo(byte[] header)
        {
            for (int i = 0; i < Signature.Length; i++)
            {
                if (header[i] != Signature[i])
                {
                    Logging.Logger.Log(Logging.SeverityGrades.Blocking, "Unexpected symbols in file header signature");
                    throw new ArgumentException("Wrong file header signature");
                }
            }
        }
    }
}
