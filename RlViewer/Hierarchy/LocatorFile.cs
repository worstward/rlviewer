using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Hierarchy
{
    public abstract class LocatorFile : LoadedFile
    {
        protected LocatorFile(FileProperties properties) : base(properties)
        {
        }

    }
}
