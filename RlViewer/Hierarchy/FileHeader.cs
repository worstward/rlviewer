using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Hierarchy
{
    public abstract class FileHeader
    {
        public abstract byte[] Sig { get; }
        public abstract int HeaderLength { get; }
    }
}
