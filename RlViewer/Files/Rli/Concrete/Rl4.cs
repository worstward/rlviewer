using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RlViewer.Behaviors.Draw;
using RlViewer.Files.Rli.Abstract;
using RlViewer.Headers.Abstract;
using RlViewer.Headers.Concrete.Rl4;


namespace RlViewer.Files.Rli.Concrete
{
    public class Rl4 : RliFile
    {
        public Rl4(FileProperties properties) : base(properties)
        {
            _header = new Rl4Header(properties.FilePath);
        }

        private Rl4Header _header;

        public override FileHeader Header
        {
            get { return _header; }
        }

        public override int Height
        {
            get
            {
                return _header.HeaderStruct.rlParams.height;    
            }
        }

        public override int Width
        {
            get
            {
                return _header.HeaderStruct.rlParams.width;
            }
        }


    }
}
