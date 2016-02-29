using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.Filters
{
    interface ILuTable
    {
        byte[] InitLut(int step);
        byte[] LuTable { get; }
        
    }
}
