using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Behaviors;

namespace RlViewer.Hierarchy.Rhg.Abstract
{
    public abstract class RhgFile : LocatorFile, IHeader
    {
        protected RhgFile(FileProperties properties) : base(properties)
        {

        }

        public abstract byte[] ReadFileHeader();
        public abstract FileHeader Header { get; }

        public override abstract bool CheckFile();
    }
}
