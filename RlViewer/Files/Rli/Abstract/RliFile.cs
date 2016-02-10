using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RlViewer.Headers.Abstract;
using RlViewer.Behaviors.Draw;

namespace RlViewer.Files.Rli.Abstract
{
    public abstract class RliFile : LocatorFile, IHeader
    {
        protected RliFile(FileProperties properties) : base(properties)
        {

        }

        public override abstract FileHeader Header { get; }

        public abstract int Width { get; }
        public abstract int Height { get; }


    }
}
