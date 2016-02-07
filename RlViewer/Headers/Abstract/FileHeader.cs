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
        public abstract byte[] Signature { get; }

        /// <summary>
        /// Gets length of this file header
        /// </summary>
        public abstract int HeaderLength { get; }

        /// <summary>
        /// Returns parsed info from file header
        /// </summary>
        /// <returns>Parsed header info instance</returns>
        public abstract Task<HeaderInfoOutput[]> GetHeaderInfo();



    }
}
