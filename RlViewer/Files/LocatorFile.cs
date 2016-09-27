using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete;

namespace RlViewer.Files
{

    /// <summary>
    /// Incapsulates radiolocation file
    /// </summary>
    public abstract class LocatorFile : LoadedFile
    {
        protected LocatorFile(FileProperties properties, Headers.Abstract.LocatorFileHeader header, RlViewer.Navigation.NavigationContainer navi)
            : base(properties)
        {

        }

        /// <summary>
        /// Contains navigation data of each data string header
        /// </summary>
        public abstract Navigation.NavigationContainer Navigation { get; }
        

        public abstract LocatorFileHeader Header { get; }

        /// <summary>
        /// Amount of samples in data string
        /// </summary>
        public abstract int Width { get; }

        /// <summary>
        /// Amount of data strings in a file
        /// </summary>
        public abstract int Height { get; protected set; }

        /// <summary>
        /// Increases image height by provided amount
        /// </summary>
        public void ExpandImageHeight(int delta)
        {
            if (delta < 0) 
                throw new ArgumentException("delta");

            Height += delta;
        }
    }
}
