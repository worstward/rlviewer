using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Files;
using RlViewer.Files.Rli.Concrete;



namespace RlViewer.Factories
{
    class Rl4Factory : FileFactory
    {
        public override LoadedFile Create(FileProperties properties)
        {
            return new Rl4(properties);
        }
    }
}
