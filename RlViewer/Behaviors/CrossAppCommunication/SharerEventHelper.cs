using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.CrossAppCommunication
{
    public delegate void GotDataEventHandler(object sender, GotDataEventArgs e);

    public class GotDataEventArgs : EventArgs
    {
        public GotDataEventArgs(byte[] data)
        {
            _data = data;
        }
        
        private byte[] _data;
        public byte[] Data
        {
            get { return _data; }
        }
    }

}
