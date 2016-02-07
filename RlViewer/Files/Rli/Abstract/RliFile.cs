using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Abstract;

namespace RlViewer.Files.Rli.Abstract
{
    public abstract class RliFile : LocatorFile, IHeader
    {
        protected RliFile(FileProperties properties) : base(properties)
        {
          // ReadFileHeaderAsync().Wait();
        }

        public override abstract FileHeader Header { get; }

    }
}
