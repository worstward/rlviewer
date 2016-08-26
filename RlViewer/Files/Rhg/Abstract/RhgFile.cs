using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Abstract;

namespace RlViewer.Files.Rhg.Abstract
{

    /// <summary>
    /// Incapsulates radiohologram file
    /// </summary>
    public abstract class RhgFile : LocatorFile
    {
        protected RhgFile(FileProperties properties, Headers.Abstract.LocatorFileHeader header, RlViewer.Navigation.NavigationContainer navi)
            : base(properties, header, navi)
        {

        }

        public override abstract LocatorFileHeader Header { get; }

    }
}
