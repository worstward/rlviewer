using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Files.Rli.Abstract;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete.Raw;


namespace RlViewer.Files.Rli.Concrete
{
    public class Raw : RliFile
    {
        public Raw(FileProperties properties) : base(properties)
        {
            _header = new RawHeader(properties.FilePath);
        }

        private FileHeader _header;

        public override FileHeader Header
        {
            get { return _header; }
        }

        public override int Height
        {
            get { return 12000; }
        }
        public override int Width
        {
            get { return 12000; }
        }

    }
}

