using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Files
{
    /// <summary>
    /// Incapsulates any file that's supported
    /// </summary>
    public abstract class LoadedFile
    {

        protected LoadedFile(FileProperties properties)
        {
            Properties = properties;
        }

        /// <summary>
        /// This file common properties
        /// </summary>
        public FileProperties Properties { get; private set; }

    }
}
