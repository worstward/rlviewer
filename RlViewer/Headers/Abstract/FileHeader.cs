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
        public abstract int HeaderLength { get; }

        /// <summary>
        /// Returns parsed info from file header
        /// </summary>
        /// <returns>Array of parsed subheaders info</returns>
        public abstract HeaderInfoOutput[] GetHeaderInfo();

    }
}
