using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Files.Rli.Abstract;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete;


namespace RlViewer.Files.Rli.Concrete
{
    public class Rl4 : RliFile
    {
        public Rl4(FileProperties properties) : base(properties)
        {
            _header = new Rl4Header(properties.FilePath);
        }

        private FileHeader _header;

        public override FileHeader Header
        {
            get { return _header; }
        }

    }
}
