using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Abstract;

namespace RlViewer.Files.Rhg.Abstract
{
    public abstract class RhgFile : LocatorFile
    {
        protected RhgFile(FileProperties properties) : base(properties)
        {

        }
        public override abstract FileHeader Header { get; }

    }
}
