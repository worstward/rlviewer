using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Headers.Abstract
{
    public abstract class FileHeader
    {
        public abstract byte[] Signature { get; }
        public abstract int HeaderLength { get; }

        public abstract byte[] FillHeader(string path);
    }
}
