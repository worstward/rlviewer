using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Headers.Concrete.Rl8
{
    class Rl8Header : Rl4.Rl4Header
    {
        public Rl8Header(string path) : base(path)
        {

        }

        private int _bytesPerSample = 8;
        public override int BytesPerSample
        {
            get
            {
                return _bytesPerSample;
            }
        }

    }
}
