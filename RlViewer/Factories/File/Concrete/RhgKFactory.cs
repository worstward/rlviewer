using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Files;
using RlViewer.Files.Rhg.Concrete;
using RlViewer.Factories.File.Abstract;

namespace RlViewer.Factories.File.Concrete
{
    class RhgKFactory : FileFactory
    {
        public override LoadedFile Create(FileProperties properties)
        {
            return new RhgK(properties);
        }
    }
}
