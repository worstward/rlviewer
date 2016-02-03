using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Hierarchy;


namespace RlViewer.Behaviors
{
    interface IHeader
    {
        byte[] ReadFileHeader();
        FileHeader Header { get; }

    }
}
