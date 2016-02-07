using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Files
{
    public abstract class LoadedFile
    {

        protected LoadedFile(FileProperties properties)
        {
            Properties = properties;
        }

        public FileProperties Properties { get; private set; }

    }
}
