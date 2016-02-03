using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Behaviors;


namespace RlViewer.Hierarchy.Rli.Abstract
{
    public abstract class RliFile : LocatorFile, IHeader
    {
        protected RliFile(FileProperties properties) : base(properties)
        {
          // ReadFileHeaderAsync().Wait();
        }

        public abstract byte[] ReadFileHeader();
        public abstract FileHeader Header { get; }

        public override abstract bool CheckFile();
    }
}
