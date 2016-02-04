using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Files;
using RlViewer.Files.Rhg.Concrete;

namespace RlViewer.Factories
{
    class RhgKFactory : FileFactory
    {
        public override LoadedFile Create(FileProperties properties)
        {
            return new RhgK(properties);
        }
    }
}
