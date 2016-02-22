using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Files.Rhg.Abstract;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete;

namespace RlViewer.Files.Rhg.Concrete
{
    public class RhgK : RhgFile
    {
        public RhgK(FileProperties properties) : base(properties)
        {
            _header = new RhgKHeader(properties.FilePath);
        }

        private FileHeader _header;

        public override FileHeader Header
        {
            get { return _header; }
        }

        public override int Width
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public override int Height
        {
            get
            {
                throw new NotImplementedException();
            }
        }


    }
}
